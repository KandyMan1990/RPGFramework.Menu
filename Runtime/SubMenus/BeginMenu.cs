using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Data;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Settings;
using RPGFramework.Core.Store;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;
        private readonly ISettingsService     m_SettingsService;
        private readonly IMusicPlayer         m_MusicPlayer;
        private readonly ISfxPlayer           m_SfxPlayer;
        private readonly ICurrentModuleStore  m_CurrentModuleStore;
        private readonly IChangeModuleStore   m_ChangeModuleStore;
        private readonly IPlayTimeStore       m_PlayTimeStore;

        private bool m_StartingNewGame;

        public BeginMenu(ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISettingsService     settingsService,
                         IMusicPlayer         musicPlayer,
                         ISfxPlayer           sfxPlayer,
                         ICurrentModuleStore  currentModuleStore,
                         IChangeModuleStore   changeModuleStore,
                         IPlayTimeStore       playTimeStore,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         IMenuModule          menuModule,
                         IAudioIntentPlayer   audioIntentPlayer) : base(beginMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
            m_SettingsService     = settingsService;
            m_MusicPlayer         = musicPlayer;
            m_SfxPlayer           = sfxPlayer;
            m_CurrentModuleStore  = currentModuleStore;
            m_ChangeModuleStore   = changeModuleStore;
            m_PlayTimeStore       = playTimeStore;
        }

        protected override Task OnEnterComplete()
        {
            return ApplySettingsAsync();
        }

        protected override Task OnResumeAsync()
        {
            if (m_StartingNewGame)
            {
                m_AudioIntentPlayer.Play(AudioIntent.NewGame, AudioContext.Menu);

                m_PlayTimeStore.StartCounting();

                m_MenuModule.RequestModuleChange();
                return Task.CompletedTask;
            }

            return base.OnResumeAsync();
        }

        protected override void RegisterCallbacks()
        {
            m_MenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_MenuUI.OnLoadGamePressed += OnLoadGamePressed;
            m_MenuUI.OnQuitPressed     += OnQuitPressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnQuitPressed     -= OnQuitPressed;
            m_MenuUI.OnLoadGamePressed -= OnLoadGamePressed;
            m_MenuUI.OnNewGamePressed  -= OnNewGamePressed;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            return false;
        }

        private void OnNewGamePressed()
        {
            string filename = m_SaveDataService.GetUnusedSaveFileName();

            m_SaveDataService.BeginSave(filename);

            m_ChangeModuleStore.SetModuleId(m_CurrentModuleStore.ModuleId);

            m_StartingNewGame = true;

            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

        private void OnLoadGamePressed()
        {
            if (m_SaveDataService.GetListOfSaveFiles().Length == 0)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_MenuModule.PushMenu(MenuType.Load).FireAndForget();
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        private Task ApplySettingsAsync()
        {
            m_SettingsService.TryGetSection(FrameworkSettingsSections.CONFIG_DATA, out SaveSection<ConfigData_V1> configData);

            ConfigData_V1 data = configData.Data;

            m_MusicPlayer.SetVolume(data.MusicVolume);
            m_SfxPlayer.SetVolume(data.SfxVolume);

            if (!m_SettingsService.IsSaved)
            {
                return m_MenuModule.PushMenu(MenuType.Language);
            }

            string language = data.GetLanguage();

            return m_LocalisationService.SetCurrentLanguage(language);
        }
    }
}