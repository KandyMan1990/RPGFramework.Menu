using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using RPGFramework.Core;
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

        private readonly IMenuModule          m_MenuModule;
        private readonly IBeginMenuUI         m_BeginMenuUI;
        private readonly ILocalisationService m_LocalisationService;
        private readonly ISaveDataService     m_SaveDataService;
        private readonly ISaveFactory         m_SaveFactory;

        public BeginMenu(IMenuModule          menuModule,
                         IBeginMenuUI         beginMenuUI,
                         IInputRouter         inputRouter,
                         ILocalisationService localisationService,
                         ISaveDataService     saveDataService,
                         ISaveFactory         saveFactory) : base(beginMenuUI, inputRouter)
        {
            m_MenuModule          = menuModule;
            m_BeginMenuUI         = beginMenuUI;
            m_LocalisationService = localisationService;
            m_SaveDataService     = saveDataService;
            m_SaveFactory         = saveFactory;
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await base.OnEnterAsync(args);

            await SetLanguageAsync();
        }

        protected override void RegisterCallbacks()
        {
            m_BeginMenuUI.OnPlayAudio       += PlaySfx;
            m_BeginMenuUI.OnNewGamePressed  += OnNewGamePressed;
            m_BeginMenuUI.OnLoadGamePressed += OnLoadGamePressed;
            m_BeginMenuUI.OnSettingsPressed += OnSettingsPressed;
            m_BeginMenuUI.OnQuitPressed     += OnQuitPressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_BeginMenuUI.OnQuitPressed     -= OnQuitPressed;
            m_BeginMenuUI.OnSettingsPressed -= OnSettingsPressed;
            m_BeginMenuUI.OnLoadGamePressed -= OnLoadGamePressed;
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
            string filename = m_SaveDataService.GetUnusedSaveFileName();

            m_SaveDataService.BeginSave(filename);

            m_SaveFactory.CreateDefaultSave(m_SaveDataService);

            m_SaveDataService.CommitSave();

            m_MenuModule.ReturnToPreviousModuleAsync().FireAndForget();
        }

        private void OnLoadGamePressed()
        {
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

        private async Task SetLanguageAsync()
        {
            ConfigData_V1 data;

            if (m_SaveDataService.TryGetLastWrittenSaveFileName(out string filename))
            {
                m_SaveDataService.BeginSave(filename);

                if (m_SaveDataService.TryGetSection(FrameworkSaveSectionDatabase.CONFIG_DATA, out SaveSection<ConfigData_V1> configData))
                {
                    data = configData.Data;
                    m_SaveDataService.ClearSaveDataFromMemory();
                }
                else
                {
                    throw new InvalidDataException($"{nameof(IBeginMenu)}::{nameof(SetLanguageAsync)} Save file [{filename}] does not contain config data");
                }
            }
            else
            {
                // TODO: push a language select popup here instead
                string[] languages = await m_LocalisationService.GetAllLanguages();

                int languageIndex = Array.IndexOf(languages, CultureInfo.CurrentCulture.Name);

                string newLanguage = languageIndex >= 0 ? languages[languageIndex] : "en-GB";

                data = new ConfigData_V1();
                data.SetLanguage(newLanguage);
            }

            string language = data.GetLanguage();
            await m_LocalisationService.SetCurrentLanguage(language);
        }
    }
}