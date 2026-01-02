using System;
using RPGFramework.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using Unity.Mathematics;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class ConfigMenuUI : MenuUI<IConfigMenuUI>, IConfigMenuUI
    {
        event Action<int> IConfigMenuUI.OnLanguageChanged
        {
            add => m_OnLanguageChanged += value;
            remove => m_OnLanguageChanged -= value;
        }

        event Action IConfigMenuUI.OnControlsPressed
        {
            add => m_OnControlsPressed += value;
            remove => m_OnControlsPressed -= value;
        }

        event Action<float> IConfigMenuUI.OnMusicVolumeChanged
        {
            add => m_OnMusicVolumeChanged += value;
            remove => m_OnMusicVolumeChanged -= value;
        }

        event Action<float> IConfigMenuUI.OnSfxVolumeChanged
        {
            add => m_OnSfxVolumeChanged += value;
            remove => m_OnSfxVolumeChanged -= value;
        }

        event Action<float> IConfigMenuUI.OnBattleMessageSpeedChanged
        {
            add => m_OnBattleMessageSpeedChanged += value;
            remove => m_OnBattleMessageSpeedChanged -= value;
        }

        event Action<float> IConfigMenuUI.OnFieldMessageSpeedChanged
        {
            add => m_OnFieldMessageSpeedChanged += value;
            remove => m_OnFieldMessageSpeedChanged -= value;
        }

        private const float SLIDER_INCREMENTAL_VALUE = 0.05f;

        private event Action<int>   m_OnLanguageChanged;
        private event Action        m_OnControlsPressed;
        private event Action<float> m_OnMusicVolumeChanged;
        private event Action<float> m_OnSfxVolumeChanged;
        private event Action<float> m_OnBattleMessageSpeedChanged;
        private event Action<float> m_OnFieldMessageSpeedChanged;

        private Label       m_TitleLabel;
        private RPGUIButton m_LanguageBtn;
        private Label       m_LanguageLabel;
        private RPGUIButton m_ControlsBtn;
        private RPGUIButton m_MusicVolumeBtn;
        private Slider      m_MusicVolumeSlider;
        private RPGUIButton m_SfxVolumeBtn;
        private Slider      m_SfxVolumeSlider;
        private RPGUIButton m_BattleMessageSpeedBtn;
        private Slider      m_BattleMessageSpeedSlider;
        private RPGUIButton m_FieldMessageSpeedBtn;
        private Slider      m_FieldMessageSpeedSlider;

        protected override VisualElement GetDefaultFocusedElement() => m_LanguageBtn;

        public ConfigMenuUI(IConfigMenuLocalisationArgs localisationArgs,
                            IMenuUIProvider             uiProvider,
                            IGenericAudioIdProvider     audioIdProvider,
                            ILocalisationService        localisationService) : base(localisationArgs,
                                                                                    uiProvider,
                                                                                    audioIdProvider,
                                                                                    localisationService)
        {
        }

        void IConfigMenuUI.RefreshLocalisation()
        {
            LocaliseUI();
        }

        protected override void HookupUI()
        {
            m_TitleLabel               = m_UIInstance.Q<Label>("TitleLabel");
            m_LanguageBtn              = m_UIInstance.Q<RPGUIButton>("LanguageBtn");
            m_LanguageLabel            = m_UIInstance.Q<Label>("LanguageLabel");
            m_ControlsBtn              = m_UIInstance.Q<RPGUIButton>("ControlsBtn");
            m_MusicVolumeBtn           = m_UIInstance.Q<RPGUIButton>("MusicVolumeBtn");
            m_MusicVolumeSlider        = m_UIInstance.Q<Slider>("MusicVolumeSlider");
            m_SfxVolumeBtn             = m_UIInstance.Q<RPGUIButton>("SfxVolumeBtn");
            m_SfxVolumeSlider          = m_UIInstance.Q<Slider>("SfxVolumeSlider");
            m_BattleMessageSpeedBtn    = m_UIInstance.Q<RPGUIButton>("BattleMessageSpeedBtn");
            m_BattleMessageSpeedSlider = m_UIInstance.Q<Slider>("BattleMessageSpeedSlider");
            m_FieldMessageSpeedBtn     = m_UIInstance.Q<RPGUIButton>("FieldMessageSpeedBtn");
            m_FieldMessageSpeedSlider  = m_UIInstance.Q<Slider>("FieldMessageSpeedSlider");
        }

        protected override void LocaliseUI()
        {
            IConfigMenuLocalisationArgs args = (IConfigMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text            = m_LocalisationService.Get(args.ScreenTitle);
            m_LanguageBtn.text           = m_LocalisationService.Get(args.LanguageTitle);
            m_LanguageLabel.text         = m_LocalisationService.Get(args.Language);
            m_ControlsBtn.text           = m_LocalisationService.Get(args.Controls);
            m_MusicVolumeBtn.text        = m_LocalisationService.Get(args.MusicVolume);
            m_SfxVolumeBtn.text          = m_LocalisationService.Get(args.SfxVolume);
            m_BattleMessageSpeedBtn.text = m_LocalisationService.Get(args.BattleMessageSpeed);
            m_FieldMessageSpeedBtn.text  = m_LocalisationService.Get(args.FieldMessageSpeed);
        }

        protected override void RegisterCallbacks()
        {
            // TODO: back button to return to begin menu

            UIToolkitInputUtility.RegisterButtonCallbacks(m_LanguageBtn,           OnLanguageBtnNavigate,           null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_ControlsBtn,           OnControlsBtnNavigate,           OnControlsBtnSubmitted, OnControlsBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_MusicVolumeBtn,        OnMusicVolumeBtnNavigate,        null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_SfxVolumeBtn,          OnSfxVolumeBtnNavigate,          null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_BattleMessageSpeedBtn, OnBattleMessageSpeedBtnNavigate, null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_FieldMessageSpeedBtn,  OnFieldMessageSpeedBtnNavigate,  null,                   null,                 OnBtnFocus);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_FieldMessageSpeedBtn,  OnFieldMessageSpeedBtnNavigate,  null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_BattleMessageSpeedBtn, OnBattleMessageSpeedBtnNavigate, null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_SfxVolumeBtn,          OnSfxVolumeBtnNavigate,          null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_MusicVolumeBtn,        OnMusicVolumeBtnNavigate,        null,                   null,                 OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_ControlsBtn,           OnControlsBtnNavigate,           OnControlsBtnSubmitted, OnControlsBtnClicked, OnBtnFocus);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LanguageBtn,           OnLanguageBtnNavigate,           null,                   null,                 OnBtnFocus);

            // TODO: back button to return to begin menu
        }

        private void OnLanguageBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_LanguageBtn, m_FieldMessageSpeedBtn, m_ControlsBtn);

            if (evt.direction == NavigationMoveEvent.Direction.Left)
            {
                OnBtnFocus(null);
                m_OnLanguageChanged?.Invoke(-1);
                return;
            }

            if (evt.direction == NavigationMoveEvent.Direction.Right)
            {
                OnBtnFocus(null);
                m_OnLanguageChanged?.Invoke(1);
            }
        }

        private void OnControlsBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_ControlsBtn, m_LanguageBtn, m_MusicVolumeBtn);
        }

        private void OnMusicVolumeBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_MusicVolumeBtn, m_ControlsBtn, m_SfxVolumeBtn);

            OnSliderChanged(m_MusicVolumeSlider, evt, m_OnMusicVolumeChanged);
        }

        private void OnSfxVolumeBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_SfxVolumeBtn, m_MusicVolumeBtn, m_BattleMessageSpeedBtn);

            OnSliderChanged(m_SfxVolumeSlider, evt, m_OnSfxVolumeChanged);
        }

        private void OnBattleMessageSpeedBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_BattleMessageSpeedBtn, m_SfxVolumeBtn, m_FieldMessageSpeedBtn);

            OnSliderChanged(m_BattleMessageSpeedSlider, evt, m_OnBattleMessageSpeedChanged);
        }

        private void OnFieldMessageSpeedBtnNavigate(NavigationMoveEvent evt)
        {
            UIToolkitInputUtility.Navigate(evt, m_FieldMessageSpeedBtn, m_BattleMessageSpeedBtn, m_LanguageBtn);

            OnSliderChanged(m_FieldMessageSpeedSlider, evt, m_OnFieldMessageSpeedChanged);
        }

        private void OnControlsBtnSubmitted(NavigationSubmitEvent evt)
        {
            OnControlsBtnCallback();
        }

        private void OnControlsBtnClicked(ClickEvent evt)
        {
            OnControlsBtnCallback();
        }

        private void OnControlsBtnCallback()
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
            m_OnControlsPressed?.Invoke();
        }

        private void OnSliderChanged(Slider slider, NavigationMoveEvent evt, Action<float> action)
        {
            const float epsilon = 0.001f;
            float       value   = slider.value;

            if (evt.direction == NavigationMoveEvent.Direction.Left)
            {
                value = math.clamp(value - SLIDER_INCREMENTAL_VALUE, 0f, 1f);
            }
            else if (evt.direction == NavigationMoveEvent.Direction.Right)
            {
                value = math.clamp(value + SLIDER_INCREMENTAL_VALUE, 0f, 1f);
            }

            if (math.abs(slider.value - value) > epsilon)
            {
                OnBtnFocus(null);
                slider.value = value;
                action?.Invoke(value);
            }
        }
    }
}