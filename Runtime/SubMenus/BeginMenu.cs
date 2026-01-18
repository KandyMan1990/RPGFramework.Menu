using System.IO;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Data;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveDataService;
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
        private readonly ICoreModule          m_CoreModule;

        public BeginMenu(IMenuModule          menuModule,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISaveFactory         saveFactory,
                         IAudioIntentPlayer   audioIntentPlayer,
                         ICoreModule          coreModule) : base(beginMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
            m_SaveFactory         = saveFactory;
            m_CoreModule          = coreModule;
        }

        protected override Task OnEnterComplete()
        {
            return SetLanguageAsync();
        }

        protected override async Task OnResumeAsync()
        {
            if (m_SaveDataService.HasSaveLoaded())
            {
                m_AudioIntentPlayer.Play(AudioIntent.NewGame, AudioContext.Menu);
                m_CoreModule.ResumeModuleAsync().FireAndForget();
                return;
            }

            await base.OnResumeAsync();
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

            m_SaveDataService.CommitSave();

            IMenuModuleArgs args = new GenericMenuModuleArgs<IConfigMenu>();

            m_MenuModule.PushMenu(args).FireAndForget();
        }

        private void OnLoadGamePressed()
        {
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
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

            IMenuModuleArgs args = new GenericMenuModuleArgs<ILanguageMenu>();
            return m_MenuModule.PushMenu(args);
        }
    }
}