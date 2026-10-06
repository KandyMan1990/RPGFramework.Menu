using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Data;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Settings;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class LanguageMenu : Menu<ILanguageMenuUI>, ILanguageMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISettingsService     m_SettingsService;

        public LanguageMenu(ILanguageMenuUI      languageMenuUI,
                            IInputRouter         inputRouter,
                            IMenuModule          menuModule,
                            ILocalisationService localisationService,
                            ISettingsService     settingsService,
                            IAudioIntentPlayer   audioIntentPlayer) : base(languageMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_LocalisationService = localisationService;
            m_SettingsService     = settingsService;
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await base.OnEnterAsync(args);

            await InitLanguageAsync();
        }

        protected override Task OnExitAsync()
        {
            m_SettingsService.TryGetSection(FrameworkSettingsSections.CONFIG_DATA, out SaveSection<ConfigData_V1> configData);

            ConfigData_V1 data = configData.Data;
            data.SetLanguage(m_LocalisationService.CurrentLanguage);

            m_SettingsService.SetSection(FrameworkSettingsSections.CONFIG_DATA, new SaveSection<ConfigData_V1>(Versions.GLOBAL_CONFIG, data));
            m_SettingsService.Commit();

            return base.OnExitAsync();
        }

        protected override void RegisterCallbacks()
        {
            m_MenuUI.OnLanguageChanged += OnLanguageChanged;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnLanguageChanged -= OnLanguageChanged;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            if (slot is ControlSlot.Primary or ControlSlot.Secondary)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private async Task InitLanguageAsync()
        {
            string[] availableLanguages = await m_LocalisationService.GetAllLanguages();

            int languageIndex = Array.IndexOf(availableLanguages, CultureInfo.CurrentCulture.Name);

            string newLanguage = languageIndex >= 0 ? availableLanguages[languageIndex] : "en-GB";

            await m_LocalisationService.SetCurrentLanguage(newLanguage);
        }

        private void OnLanguageChanged(int direction)
        {
            async Task Run()
            {
                string   currentLanguage    = m_LocalisationService.CurrentLanguage;
                string[] availableLanguages = await m_LocalisationService.GetAllLanguages();

                int languageIndex = Array.IndexOf(availableLanguages, currentLanguage);

                languageIndex = (languageIndex + availableLanguages.Length + direction) % availableLanguages.Length;

                string newLanguage = availableLanguages[languageIndex];

                await m_LocalisationService.SetCurrentLanguage(newLanguage);

                m_MenuUI.RefreshLocalisation();
            }

            Run().FireAndForget();
        }
    }
}