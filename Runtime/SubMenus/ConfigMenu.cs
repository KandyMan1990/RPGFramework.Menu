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
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    internal sealed class ConfigMenu : Menu<IConfigMenuUI>, IConfigMenu
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ISettingsService     m_SettingsService;
        private readonly ISettingsStore       m_SettingsStore;
        private readonly ILocalisationService m_LocalisationService;
        private readonly IMusicPlayer         m_MusicPlayer;
        private readonly ISfxPlayer           m_SfxPlayer;

        public ConfigMenu(IMenuModule          menuModule,
                          IConfigMenuUI        configMenuUI,
                          IInputRouter         inputRouter,
                          ISettingsService     settingsService,
                          ISettingsStore       settingsStore,
                          ILocalisationService localisationService,
                          IMusicPlayer         musicPlayer,
                          ISfxPlayer           sfxPlayer,
                          IAudioIntentPlayer   audioIntentPlayer) : base(configMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_SettingsService     = settingsService;
            m_SettingsStore       = settingsStore;
            m_LocalisationService = localisationService;
            m_MusicPlayer         = musicPlayer;
            m_SfxPlayer           = sfxPlayer;

            m_MenuUI.OnLanguageChanged           += OnLanguageChanged;
            m_MenuUI.OnControlsPressed           += OnControlsPressed;
            m_MenuUI.OnSliderStepped             += OnSliderStepped;
            m_MenuUI.OnMusicVolumeChanged        += OnMusicVolumeChanged;
            m_MenuUI.OnSfxVolumeChanged          += OnSfxVolumeChanged;
            m_MenuUI.OnBattleMessageSpeedChanged += OnBattleMessageSpeedChanged;
            m_MenuUI.OnFieldMessageSpeedChanged  += OnFieldMessageSpeedChanged;
        }

        protected override Task OnEnterComplete()
        {
            SetSliders();

            return base.OnEnterComplete();
        }

        protected override Task OnExitAsync()
        {
            m_SettingsService.Commit();

            return base.OnExitAsync();
        }

        protected override bool HandleAction(ulong action)
        {
            if (action == CoreActions.Cancel)
            {
                m_AudioIntentPlayer.PlayAsync(AudioIntent.Cancel, AudioContext.Menu).FireAndForget();
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private void OnLanguageChanged(int direction)
        {
            PlayNavigate();

            async Task Run()
            {
                string   currentLanguage    = m_LocalisationService.CurrentLanguage;
                string[] availableLanguages = await m_LocalisationService.GetAllLanguages();

                int index = Array.IndexOf(availableLanguages, currentLanguage);
                index = (index + availableLanguages.Length + direction) % availableLanguages.Length;

                string newLanguage = availableLanguages[index];

                await m_LocalisationService.SetCurrentLanguage(newLanguage);

                m_SettingsStore.SetLanguage(newLanguage);

                m_MenuUI.RefreshLocalisation();
            }

            Run().FireAndForget();
        }

        private void OnControlsPressed()
        {
            PlayNavigate();

            m_MenuModule.PushMenu(MenuIds.Controls).FireAndForget();
        }

        private void OnSliderStepped()
        {
            PlayNavigate();
        }

        private void PlayNavigate()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
        }

        private void OnMusicVolumeChanged(float value)
        {
            m_SettingsStore.SetMusicVolume(value);

            m_MusicPlayer.SetVolume(value);
        }

        private void OnSfxVolumeChanged(float value)
        {
            m_SettingsStore.SetSfxVolume(value);

            m_SfxPlayer.SetVolume(value);
        }

        private void OnBattleMessageSpeedChanged(float value)
        {
            m_SettingsStore.SetBattleMessageSpeed(value);
        }

        private void OnFieldMessageSpeedChanged(float value)
        {
            m_SettingsStore.SetFieldMessageSpeed(value);
        }

        private void SetSliders()
        {
            m_MenuUI.SetMusicVolume(m_SettingsStore.MusicVolume);
            m_MenuUI.SetSfxVolume(m_SettingsStore.SfxVolume);
            m_MenuUI.SetBattleMessageSpeed(m_SettingsStore.BattleMessageSpeed);
            m_MenuUI.SetFieldMessageSpeed(m_SettingsStore.FieldMessageSpeed);
        }
    }
}
