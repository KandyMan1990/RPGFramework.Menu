using System;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Settings;
using RPGFramework.Core.Store;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    internal sealed class LanguageMenu : Menu<ILanguageMenuUI>, ILanguageMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ILocalisationService m_LocalisationService;
        private readonly ISettingsService     m_SettingsService;
        private readonly ISettingsStore       m_SettingsStore;

        public LanguageMenu(ILanguageMenuUI      languageMenuUI,
                            IInputRouter         inputRouter,
                            IMenuModule          menuModule,
                            ILocalisationService localisationService,
                            ISettingsService     settingsService,
                            ISettingsStore       settingsStore,
                            ISfxPlayer           sfxPlayer,
                            ISystemSoundConfig   systemSounds) : base(languageMenuUI, inputRouter, menuModule, sfxPlayer, systemSounds)
        {
            m_LocalisationService = localisationService;
            m_SettingsService     = settingsService;
            m_SettingsStore       = settingsStore;

            m_MenuUI.OnLanguageChanged += OnLanguageChanged;
        }

        protected override Task OnExitAsync()
        {
            m_SettingsStore.SetLanguage(m_LocalisationService.CurrentLanguage);
            m_SettingsService.Commit();

            return base.OnExitAsync();
        }

        protected override bool HandleAction(ulong action)
        {
            if (action == MenuActions.Submit || action == CoreActions.Cancel)
            {
                PlaySound(m_SystemSounds.Cursor);
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private void OnLanguageChanged(int direction)
        {
            PlaySound(m_SystemSounds.Cursor);

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