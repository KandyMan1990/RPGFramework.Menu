using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Data;
using RPGFramework.Core.GlobalConfig;
using RPGFramework.Core.Input;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class ConfigMenu : Menu<IConfigMenuUI>, IConfigMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly IMenuModule          m_MenuModule;
        private readonly IConfigMenuUI        m_ConfigMenuUI;
        private readonly IGlobalConfig        m_GlobalConfig;
        private readonly ILocalisationService m_LocalisationService;

        private ConfigData_V1 m_ConfigData;

        public ConfigMenu(IMenuModule          menuModule,
                          IConfigMenuUI        configMenuUI,
                          IInputRouter         inputRouter,
                          IGlobalConfig        globalConfig,
                          ILocalisationService localisationService) : base(configMenuUI, inputRouter)
        {
            m_MenuModule          = menuModule;
            m_ConfigMenuUI        = configMenuUI;
            m_GlobalConfig        = globalConfig;
            m_LocalisationService = localisationService;
        }

        protected override Task OnEnterAsync(Dictionary<string, object> args)
        {
            if (!m_GlobalConfig.TryGetSection(GlobalConfigKeys.CORE, Versions.GLOBAL_CONFIG, out ConfigData_V1 configData, out uint storedVersion))
            {
                throw new InvalidDataException($"{nameof(ConfigMenu)}::{nameof(OnEnterAsync)} Failed to get global config data");
            }

            m_ConfigData = configData;

            return base.OnEnterAsync(args);
        }

        protected override Task OnEnterComplete()
        {
            SetSliders();

            return base.OnEnterComplete();
        }

        protected override Task OnExitAsync()
        {
            m_GlobalConfig.SetSection(GlobalConfigKeys.CORE, Versions.GLOBAL_CONFIG, m_ConfigData);

            return base.OnExitAsync();
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

        protected override bool HandleControl(ControlSlot slot)
        {
            if (slot == ControlSlot.Secondary)
            {
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
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

                m_ConfigData.SetLanguage(newLanguage);

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
            m_ConfigData.MusicVolume = value;
        }

        private void OnSfxVolumeChanged(float value)
        {
            m_ConfigData.SfxVolume = value;
        }

        private void OnBattleMessageSpeedChanged(float value)
        {
            m_ConfigData.BattleMessageSpeed = value;
        }

        private void OnFieldMessageSpeedChanged(float value)
        {
            m_ConfigData.FieldMessageSpeed = value;
        }

        private void SetSliders()
        {
            m_ConfigMenuUI.SetMusicVolume(m_ConfigData.MusicVolume);
            m_ConfigMenuUI.SetSfxVolume(m_ConfigData.SfxVolume);
            m_ConfigMenuUI.SetBattleMessageSpeed(m_ConfigData.BattleMessageSpeed);
            m_ConfigMenuUI.SetFieldMessageSpeed(m_ConfigData.FieldMessageSpeed);
        }
    }
}