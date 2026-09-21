using System.IO;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Data;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;
        private readonly ISaveFactory         m_SaveFactory;
        private readonly ICurrentModuleStore  m_CurrentModuleStore;
        private readonly IChangeModuleStore   m_ChangeModuleStore;

        public BeginMenu(ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISaveFactory         saveFactory,
                         ICurrentModuleStore  currentModuleStore,
                         IChangeModuleStore   changeModuleStore,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         IMenuModule          menuModule,
                         IAudioIntentPlayer   audioIntentPlayer) : base(beginMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
            m_SaveFactory         = saveFactory;
            m_CurrentModuleStore  = currentModuleStore;
            m_ChangeModuleStore   = changeModuleStore;
        }

        protected override Task OnEnterComplete()
        {
            return SetLanguageAsync();
        }

        protected override Task OnResumeAsync()
        {
            if (m_SaveDataService.HasSaveLoaded())
            {
                m_AudioIntentPlayer.Play(AudioIntent.NewGame, AudioContext.Menu);

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

            m_SaveFactory.CreateDefaultSave(m_SaveDataService);

            m_ChangeModuleStore.SetModuleId(m_CurrentModuleStore.GetModuleId);

            m_SaveDataService.CommitSave();

            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

        private void OnLoadGamePressed()
        {
            // TODO: Temporary.
            // Loads the most recently written save. A save-slot picker belongs in the Save menu, which is
            // one of the MenuType values that is not implemented yet.
            if (!m_SaveDataService.TryGetLastWrittenSaveFileName(out string filename))
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_SaveDataService.BeginSave(filename);
            m_SaveFactory.OnSaveLoaded(m_SaveDataService);

            m_ChangeModuleStore.SetModuleId(m_CurrentModuleStore.GetModuleId);

            m_AudioIntentPlayer.Play(AudioIntent.LoadGame, AudioContext.Menu);

            m_MenuModule.RequestModuleChange();
            m_MenuModule.PopMenu().FireAndForget();
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        private Task SetLanguageAsync()
        {
            if (m_SaveDataService.TryGetLastWrittenSaveFileName(out string filename))
            {
                m_SaveDataService.BeginSave(filename);

                if (m_SaveDataService.TryGetSection(FrameworkSaveSectionDatabase.CONFIG_DATA, out SaveSection<ConfigData_V1> configData))
                {
                    ConfigData_V1 data = configData.Data;
                    m_SaveDataService.ClearSaveDataFromMemory();

                    string language = data.GetLanguage();
                    return m_LocalisationService.SetCurrentLanguage(language);
                }

                throw new InvalidDataException($"{nameof(IBeginMenu)}::{nameof(SetLanguageAsync)} Save file [{filename}] does not contain config data");
            }

            return m_MenuModule.PushMenu(MenuType.Language);
        }
    }
}