using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class LanguageMenu : Menu<ILanguageMenuUI>, ILanguageMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly ILocalisationService    m_LocalisationService;

        public LanguageMenu(ILanguageMenuUI         menuUI,
                            IInputRouter            inputRouter,
                            IMenuModule             menuModule,
                            ILocalisationService    localisationService,
                            IGenericAudioIdProvider audioIdProvider) : base(menuUI, inputRouter, menuModule, audioIdProvider)
        {
            m_LocalisationService = localisationService;
        }

        protected override async Task OnEnterAsync(Dictionary<string, object> args)
        {
            await base.OnEnterAsync(args);

            await InitLanguageAsync();
        }

        protected override void RegisterCallbacks()
        {
            m_MenuUI.OnPlayAudio       += PlaySfx;
            m_MenuUI.OnLanguageChanged += OnLanguageChanged;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnLanguageChanged -= OnLanguageChanged;
            m_MenuUI.OnPlayAudio       -= PlaySfx;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            if (slot is ControlSlot.Primary or ControlSlot.Secondary)
            {
                m_MenuModule.PlaySfx(m_AudioIdProvider.ButtonNavigate);
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