using System;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class ConfigMenu : Menu<IConfigMenuUI>, IConfigMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly IMenuModule          m_MenuModule;
        private readonly IConfigMenuUI        m_ConfigMenuUI;
        private readonly ILocalisationService m_LocalisationService;

        public ConfigMenu(IMenuModule          menuModule,
                          ILocalisationService localisationService,
                          IConfigMenuUI        configMenuUI) : base(configMenuUI)
        {
            m_MenuModule          = menuModule;
            m_LocalisationService = localisationService;
            m_ConfigMenuUI        = configMenuUI;
        }
        protected override void RegisterCallbacks()
        {
            m_ConfigMenuUI.OnPlayAudio                 += PlaySfx;
            m_ConfigMenuUI.OnLanguageChanged           += OnLanguageChanged;
            m_ConfigMenuUI.OnControlsPressed           += OnControlsPressed;
            m_ConfigMenuUI.OnMusicVolumeChanged        += OnMusicVolumeChanged;
            m_ConfigMenuUI.OnSfxVolumeChanged          += OnSfxVolumeChanged;
            m_ConfigMenuUI.OnBattleMessageSpeedChanged += OnBattleMessageSpeedChanged;
            m_ConfigMenuUI.OnFieldMessageSpeedChanged  += OnFieldMessageSpeedChanged;
        }

        protected override void UnregisterCallbacks()
        {
            m_ConfigMenuUI.OnFieldMessageSpeedChanged  -= OnFieldMessageSpeedChanged;
            m_ConfigMenuUI.OnBattleMessageSpeedChanged -= OnBattleMessageSpeedChanged;
            m_ConfigMenuUI.OnSfxVolumeChanged          -= OnSfxVolumeChanged;
            m_ConfigMenuUI.OnMusicVolumeChanged        -= OnMusicVolumeChanged;
            m_ConfigMenuUI.OnControlsPressed           -= OnControlsPressed;
            m_ConfigMenuUI.OnLanguageChanged           -= OnLanguageChanged;
            m_ConfigMenuUI.OnPlayAudio                 -= PlaySfx;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }

        private void OnLanguageChanged(int direction)
        {
            async Task Run()
            {
                string   currentLanguage    = m_LocalisationService.CurrentLanguage;
                string[] availableLanguages = await m_LocalisationService.GetAllLanguages();

                int index = Array.IndexOf(availableLanguages, currentLanguage);
                index = (index + availableLanguages.Length + direction) % availableLanguages.Length;

                string newLanguage = availableLanguages[index];

                await m_LocalisationService.SetCurrentLanguage(newLanguage);

                m_ConfigMenuUI.RefreshLocalisation();
            }

            Run().FireAndForget();
        }

        private void OnControlsPressed()
        {
            // TODO: open a page to configure the controls
            UnityEngine.Debug.Log("OnControlsPressed");
        }

        private void OnMusicVolumeChanged(float value)
        {
            // TODO: store in a save file
            UnityEngine.Debug.Log("OnMusicVolumeChanged: " + value);
        }

        private void OnSfxVolumeChanged(float value)
        {
            // TODO: store in a save file
            UnityEngine.Debug.Log("OnSfxVolumeChanged: " + value);
        }

        private void OnBattleMessageSpeedChanged(float value)
        {
            // TODO: store in a save file
            UnityEngine.Debug.Log("OnBattleMessageSpeedChanged: " + value);
        }

        private void OnFieldMessageSpeedChanged(float value)
        {
            // TODO: store in a save file
            UnityEngine.Debug.Log("OnFieldMessageSpeedChanged: " + value);
        }
    }
}