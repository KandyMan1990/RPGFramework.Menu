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
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    public sealed class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;
        private readonly ISettingsService     m_SettingsService;
        private readonly IMusicPlayer         m_MusicPlayer;
        private readonly ISfxPlayer           m_SfxPlayer;
        private readonly IPlayTimeStore       m_PlayTimeStore;

        private bool m_StartingNewGame;

        public BeginMenu(ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISettingsService     settingsService,
                         IMusicPlayer         musicPlayer,
                         ISfxPlayer           sfxPlayer,
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
            m_PlayTimeStore       = playTimeStore;

            m_MenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_MenuUI.OnLoadGamePressed += OnLoadGamePressed;
            m_MenuUI.OnQuitPressed     += OnQuitPressed;

#if ENABLE_RPG_FRAMEWORK_DEBUG
            // A game's own title UI may not offer the room.
            if (m_MenuUI is IDebugBeginMenuUI debugUI)
            {
                debugUI.OnDebugRoomPressed += OnDebugRoomPressed;
            }
#endif
        }

        protected override Task OnEnterComplete()
        {
            m_PlayTimeStore.StopCounting();

            return ApplySettingsAsync();
        }

        protected override Task OnResumeAsync()
        {
            if (m_StartingNewGame)
            {
                m_PlayTimeStore.StartCounting();

                m_MenuModule.RequestModuleChange(MenuOutcomes.PLAYTHROUGH);
                return Task.CompletedTask;
            }

            return base.OnResumeAsync();
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            return false;
        }

        private void OnNewGamePressed()
        {
            string filename = m_SaveDataService.GetUnusedSaveFileName();

            m_SaveDataService.BeginSave(filename);

            m_StartingNewGame = true;

            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

        private void OnLoadGamePressed()
        {
            if (m_SaveDataService.GetListOfSaveFiles().Length == 0)
            {
                m_AudioIntentPlayer.PlayAsync(AudioIntent.Error, AudioContext.Menu).FireAndForget();
                return;
            }

            m_MenuModule.PushMenu(MenuType.Load).FireAndForget();
        }

#if ENABLE_RPG_FRAMEWORK_DEBUG
        // The room plays a playthrough begun here, as New Game begins one.
        private void OnDebugRoomPressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();

            m_SaveDataService.BeginSave(m_SaveDataService.GetUnusedSaveFileName());

            m_MenuModule.RequestModuleChange(DebugMenuOutcomes.DEBUG_ROOM);
            m_MenuModule.PopMenu().FireAndForget();
        }
#endif

        private void OnQuitPressed(Task cancelSound)
        {
            QuitAsync(cancelSound).FireAndForget();
        }

        // The cancel sound plays out before the game closes: quitting at once also cut its bundle's load short, which
        // in the editor left the bundle loaded into the next play session. It quits even if the sound failed, which is
        // logged as it passes on.
        private async Task QuitAsync(Task cancelSound)
        {
            SetInputEnabled(false);

            try
            {
                await cancelSound;
            }
            finally
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
            }
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