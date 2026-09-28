using System;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class PartyMenuUI : MenuUI<IPartyMenuUI>, IPartyMenuUI
    {
        event Action IPartyMenuUI.OnConfigPressed
        {
            add => m_OnConfigPressed += value;
            remove => m_OnConfigPressed -= value;
        }

        event Action IPartyMenuUI.OnSavePressed
        {
            add => m_OnSavePressed += value;
            remove => m_OnSavePressed -= value;
        }

        private const float DISABLED_OPACITY = 0.5f;

        private event Action m_OnConfigPressed;
        private event Action m_OnSavePressed;

        private RPGUIButton m_ConfigBtn;
        private RPGUIButton m_SaveBtn;
        private Label       m_LocationLabel;
        private Label       m_TimeLabel;
        private Label       m_PlayTimeLabel;

        private ulong m_LocationName;

        protected override VisualElement GetDefaultFocusedElement() => m_ConfigBtn;

        public PartyMenuUI(IPartyMenuLocalisationArgs localisationArgs,
                           IMenuUIProvider            uiProvider,
                           IAudioIntentPlayer         audioIntentPlayer,
                           ILocalisationService       localisationService) : base(localisationArgs, uiProvider, audioIntentPlayer, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_ConfigBtn     = m_UIInstance.Q<RPGUIButton>("ConfigBtn");
            m_SaveBtn       = m_UIInstance.Q<RPGUIButton>("SaveBtn");
            m_LocationLabel = m_UIInstance.Q<Label>("LocationLabel");
            m_TimeLabel     = m_UIInstance.Q<Label>("TimeLabel");
            m_PlayTimeLabel = m_UIInstance.Q<Label>("PlayTimeLabel");
        }

        protected override void LocaliseUI()
        {
            IPartyMenuLocalisationArgs args = (IPartyMenuLocalisationArgs)m_LocalisationArgs;

            m_ConfigBtn.text = m_LocalisationService.Get(args.Config);
            m_SaveBtn.text   = m_LocalisationService.Get(args.Save);
            m_TimeLabel.text = m_LocalisationService.Get(args.Time);

            m_LocationLabel.text = m_LocalisationService.TryGet(m_LocationName, out string locationName) ? locationName : string.Empty;
        }

        protected override void RegisterCallbacks()
        {
            UIToolkitInputUtility.RegisterButtonCallbacks(m_ConfigBtn, OnConfigBtnNavigate, OnConfigBtnSubmitted, OnConfigBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_SaveBtn,   OnSaveBtnNavigate,   OnSaveBtnSubmitted,   OnSaveBtnClicked);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_SaveBtn,   OnSaveBtnNavigate,   OnSaveBtnSubmitted,   OnSaveBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_ConfigBtn, OnConfigBtnNavigate, OnConfigBtnSubmitted, OnConfigBtnClicked);
        }

        void IPartyMenuUI.SetSaveEnabled(bool enabled)
        {
            // Still focusable when greyed out, so choosing it can say no.
            m_SaveBtn.style.opacity = enabled ? 1f : DISABLED_OPACITY;
        }

        void IPartyMenuUI.SetLocationName(ulong keyHash)
        {
            m_LocationName = keyHash;

            LocaliseUI();
        }

        void IPartyMenuUI.SetPlayTime(uint seconds)
        {
            m_PlayTimeLabel.text = $"{seconds / 3600}:{seconds / 60 % 60:00}";
        }

        private void OnConfigBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_ConfigBtn, m_SaveBtn, m_SaveBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnConfigBtnSubmitted(NavigationSubmitEvent evt)
        {
            m_OnConfigPressed?.Invoke();
        }

        private void OnConfigBtnClicked(ClickEvent evt)
        {
            m_OnConfigPressed?.Invoke();
        }

        private void OnSaveBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_SaveBtn, m_ConfigBtn, m_ConfigBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnSaveBtnSubmitted(NavigationSubmitEvent evt)
        {
            m_OnSavePressed?.Invoke();
        }

        private void OnSaveBtnClicked(ClickEvent evt)
        {
            m_OnSavePressed?.Invoke();
        }
    }
}