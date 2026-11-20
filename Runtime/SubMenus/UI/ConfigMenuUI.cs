using System;
using RPGFramework.Core;
using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using Unity.Mathematics;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class ConfigMenuUI : MenuUI<IConfigMenuUI>, IConfigMenuUI
    {
        event Action<int> IConfigMenuUI.OnLanguageChanged
        {
            add => m_OnLanguageChanged += value;
            remove => m_OnLanguageChanged -= value;
        }

        event Action IConfigMenuUI.OnSliderStepped
        {
            add => m_OnSliderStepped += value;
            remove => m_OnSliderStepped -= value;
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

        private const string LANGUAGE_BUTTON             = "LanguageBtn";
        private const string MUSIC_VOLUME_BUTTON         = "MusicVolumeBtn";
        private const string MUSIC_VOLUME_SLIDER         = "MusicVolumeSlider";
        private const string SFX_VOLUME_BUTTON           = "SfxVolumeBtn";
        private const string SFX_VOLUME_SLIDER           = "SfxVolumeSlider";
        private const string BATTLE_MESSAGE_SPEED_BUTTON = "BattleMessageSpeedBtn";
        private const string BATTLE_MESSAGE_SPEED_SLIDER = "BattleMessageSpeedSlider";
        private const string FIELD_MESSAGE_SPEED_BUTTON  = "FieldMessageSpeedBtn";
        private const string FIELD_MESSAGE_SPEED_SLIDER  = "FieldMessageSpeedSlider";

        private event Action<int>   m_OnLanguageChanged;
        private event Action        m_OnSliderStepped;
        private event Action<float> m_OnMusicVolumeChanged;
        private event Action<float> m_OnSfxVolumeChanged;
        private event Action<float> m_OnBattleMessageSpeedChanged;
        private event Action<float> m_OnFieldMessageSpeedChanged;

        // The rows are entries, as the cursor reaches only those.
        [LayoutElement(LANGUAGE_BUTTON)]
        private MenuEntry m_LanguageBtn;

        [LayoutElement(MUSIC_VOLUME_BUTTON)]
        private MenuEntry m_MusicVolumeBtn;

        [LayoutElement(MUSIC_VOLUME_SLIDER)]
        private Slider m_MusicVolumeSlider;

        [LayoutElement(SFX_VOLUME_BUTTON)]
        private MenuEntry m_SfxVolumeBtn;

        [LayoutElement(SFX_VOLUME_SLIDER)]
        private Slider m_SfxVolumeSlider;

        [LayoutElement(BATTLE_MESSAGE_SPEED_BUTTON)]
        private MenuEntry m_BattleMessageSpeedBtn;

        [LayoutElement(BATTLE_MESSAGE_SPEED_SLIDER)]
        private Slider m_BattleMessageSpeedSlider;

        [LayoutElement(FIELD_MESSAGE_SPEED_BUTTON)]
        private MenuEntry m_FieldMessageSpeedBtn;

        [LayoutElement(FIELD_MESSAGE_SPEED_SLIDER)]
        private Slider m_FieldMessageSpeedSlider;

        public ConfigMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_LanguageBtn              = m_UIInstance.Q<MenuEntry>(LANGUAGE_BUTTON);
            m_MusicVolumeBtn           = m_UIInstance.Q<MenuEntry>(MUSIC_VOLUME_BUTTON);
            m_MusicVolumeSlider        = m_UIInstance.Q<Slider>(MUSIC_VOLUME_SLIDER);
            m_SfxVolumeBtn             = m_UIInstance.Q<MenuEntry>(SFX_VOLUME_BUTTON);
            m_SfxVolumeSlider          = m_UIInstance.Q<Slider>(SFX_VOLUME_SLIDER);
            m_BattleMessageSpeedBtn    = m_UIInstance.Q<MenuEntry>(BATTLE_MESSAGE_SPEED_BUTTON);
            m_BattleMessageSpeedSlider = m_UIInstance.Q<Slider>(BATTLE_MESSAGE_SPEED_SLIDER);
            m_FieldMessageSpeedBtn     = m_UIInstance.Q<MenuEntry>(FIELD_MESSAGE_SPEED_BUTTON);
            m_FieldMessageSpeedSlider  = m_UIInstance.Q<Slider>(FIELD_MESSAGE_SPEED_SLIDER);

            // Its row's button takes left and right for a slider; one taking focus on a click would leave the cursor
            // with no neighbours to move by.
            m_MusicVolumeSlider.focusable        = false;
            m_SfxVolumeSlider.focusable          = false;
            m_BattleMessageSpeedSlider.focusable = false;
            m_FieldMessageSpeedSlider.focusable  = false;

            m_LanguageBtn.RegisterCallback<NavigationMoveEvent>(OnLanguageBtnNavigate);
            m_MusicVolumeBtn.RegisterCallback<NavigationMoveEvent>(OnMusicVolumeBtnNavigate);
            m_SfxVolumeBtn.RegisterCallback<NavigationMoveEvent>(OnSfxVolumeBtnNavigate);
            m_BattleMessageSpeedBtn.RegisterCallback<NavigationMoveEvent>(OnBattleMessageSpeedBtnNavigate);
            m_FieldMessageSpeedBtn.RegisterCallback<NavigationMoveEvent>(OnFieldMessageSpeedBtnNavigate);

            m_MusicVolumeSlider.RegisterValueChangedCallback(OnMusicVolumeSliderChanged);
            m_SfxVolumeSlider.RegisterValueChangedCallback(OnSfxVolumeSliderChanged);
            m_BattleMessageSpeedSlider.RegisterValueChangedCallback(OnBattleMessageSpeedSliderChanged);
            m_FieldMessageSpeedSlider.RegisterValueChangedCallback(OnFieldMessageSpeedSliderChanged);
        }

        void IConfigMenuUI.RefreshLocalisation()
        {
            Localise();
        }

        void IConfigMenuUI.SetMusicVolume(float volume)
        {
            m_MusicVolumeSlider.SetValueWithoutNotify(volume);
        }

        void IConfigMenuUI.SetSfxVolume(float volume)
        {
            m_SfxVolumeSlider.SetValueWithoutNotify(volume);
        }

        void IConfigMenuUI.SetBattleMessageSpeed(float speed)
        {
            m_BattleMessageSpeedSlider.SetValueWithoutNotify(speed);
        }

        void IConfigMenuUI.SetFieldMessageSpeed(float speed)
        {
            m_FieldMessageSpeedSlider.SetValueWithoutNotify(speed);
        }

        private void OnLanguageBtnNavigate(NavigationMoveEvent evt)
        {
            if (evt.direction == NavigationMoveEvent.Direction.Left)
            {
                m_OnLanguageChanged?.Invoke(-1);
                return;
            }

            if (evt.direction == NavigationMoveEvent.Direction.Right)
            {
                m_OnLanguageChanged?.Invoke(1);
            }
        }

        private void OnMusicVolumeBtnNavigate(NavigationMoveEvent evt)
        {
            OnSliderNavigate(m_MusicVolumeSlider, evt);
        }

        private void OnSfxVolumeBtnNavigate(NavigationMoveEvent evt)
        {
            OnSliderNavigate(m_SfxVolumeSlider, evt);
        }

        private void OnBattleMessageSpeedBtnNavigate(NavigationMoveEvent evt)
        {
            OnSliderNavigate(m_BattleMessageSpeedSlider, evt);
        }

        private void OnFieldMessageSpeedBtnNavigate(NavigationMoveEvent evt)
        {
            OnSliderNavigate(m_FieldMessageSpeedSlider, evt);
        }

        private void OnSliderNavigate(Slider slider, NavigationMoveEvent evt)
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
                m_OnSliderStepped?.Invoke();
                slider.value = value;
            }
        }

        private void OnMusicVolumeSliderChanged(ChangeEvent<float> evt)
        {
            m_OnMusicVolumeChanged?.Invoke(evt.newValue);
        }

        private void OnSfxVolumeSliderChanged(ChangeEvent<float> evt)
        {
            m_OnSfxVolumeChanged?.Invoke(evt.newValue);
        }

        private void OnBattleMessageSpeedSliderChanged(ChangeEvent<float> evt)
        {
            m_OnBattleMessageSpeedChanged?.Invoke(evt.newValue);
        }

        private void OnFieldMessageSpeedSliderChanged(ChangeEvent<float> evt)
        {
            m_OnFieldMessageSpeedChanged?.Invoke(evt.newValue);
        }
    }
}
