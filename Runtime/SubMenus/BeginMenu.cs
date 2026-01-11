using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Data;
using RPGFramework.Core.GlobalConfig;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveDataService;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly IMenuModule          m_MenuModule;
        private readonly IBeginMenuUI         m_BeginMenuUI;
        private readonly IGlobalConfig        m_GlobalConfig;
        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;

        public BeginMenu(IMenuModule          menuModule,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         IGlobalConfig        globalConfig,
                         ILocalisationService localisationService,
                         ISaveDataService     saveDataService) : base(beginMenuUI, inputRouter)
        {
            m_MenuModule          = menuModule;
            m_BeginMenuUI         = beginMenuUI;
            m_GlobalConfig        = globalConfig;
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await base.OnEnterAsync(args);

            if (!m_GlobalConfig.TryGetSection(GlobalConfigKeys.CORE, Versions.GLOBAL_CONFIG, out ConfigData_V1 configData, out uint storedVersion))
            {
                configData = new ConfigData_V1();

                string[] languages = await m_LocalisationService.GetAllLanguages();

                int languageIndex = Array.IndexOf(languages, CultureInfo.CurrentCulture.Name);

                string newLanguage = languageIndex >= 0 ? languages[languageIndex] : "en-GB";

                configData.SetLanguage(newLanguage);
                configData.MusicVolume        = 1f;
                configData.SfxVolume          = 1f;
                configData.BattleMessageSpeed = 0.5f;
                configData.FieldMessageSpeed  = 0.5f;

                m_GlobalConfig.SetSection(GlobalConfigKeys.CORE, Versions.GLOBAL_CONFIG, configData);
            }

            string language = configData.GetLanguage();

            await m_LocalisationService.SetCurrentLanguage(language);
        }

        protected override void RegisterCallbacks()
        {
            m_BeginMenuUI.OnPlayAudio       += PlaySfx;
            m_BeginMenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_BeginMenuUI.OnSettingsPressed += OnSettingsPressed;
            m_BeginMenuUI.OnQuitPressed     += OnQuitPressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_BeginMenuUI.OnQuitPressed     -= OnQuitPressed;
            m_BeginMenuUI.OnSettingsPressed -= OnSettingsPressed;
            m_BeginMenuUI.OnNewGamePressed  -= OnNewGamePressed;
            m_BeginMenuUI.OnPlayAudio       -= PlaySfx;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            return false;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }

        private void OnNewGamePressed()
        {
            object defaultSaveFile = m_SaveDataService.CreateDefaultSaveFile();
            m_SaveDataService.SetCurrentSaveFile(defaultSaveFile);

            m_MenuModule.ReturnToPreviousModuleAsync().FireAndForget();
        }

        private void OnSettingsPressed()
        {
            IMenuModuleArgs args = new MenuModuleArgs<IConfigMenu>();

            m_MenuModule.PushMenu(args).FireAndForget();
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}