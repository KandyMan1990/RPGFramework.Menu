using System;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class PartyMenuUI : MenuUI<IPartyMenuUI>, IPartyMenuUI
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

        event Action IPartyMenuUI.OnLoadPressed
        {
            add => m_OnLoadPressed += value;
            remove => m_OnLoadPressed -= value;
        }

        private const float DISABLED_OPACITY = 0.5f;

        private event Action m_OnConfigPressed;
        private event Action m_OnSavePressed;
        private event Action m_OnLoadPressed;

        private RPGUIButton m_ConfigBtn;
        private RPGUIButton m_SaveBtn;
        private RPGUIButton m_LoadBtn;
        private Label       m_PlayTimeLabel;

        private PickedLocalisedText m_LocationName;

        protected override VisualElement GetDefaultFocusedElement() => m_ConfigBtn;

        public PartyMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_ConfigBtn     = m_UIInstance.Q<RPGUIButton>("ConfigBtn");
            m_SaveBtn       = m_UIInstance.Q<RPGUIButton>("SaveBtn");
            m_LoadBtn       = m_UIInstance.Q<RPGUIButton>("LoadBtn");
            m_PlayTimeLabel = m_UIInstance.Q<Label>("PlayTimeLabel");
            m_LocationName  = (PickedLocalisedText)m_UIInstance.Q<Label>("LocationLabel").GetBinding("text");

            RPGUIButton.LinkColumn(new RPGUIButton[] { m_ConfigBtn, m_SaveBtn, m_LoadBtn }, true);

            m_ConfigBtn.Chosen += OnConfigChosen;
            m_SaveBtn.Chosen   += OnSaveChosen;
            m_LoadBtn.Chosen   += OnLoadChosen;
        }

        void IPartyMenuUI.SetSaveEnabled(bool enabled)
        {
            // Still focusable when greyed out, so choosing it can say no.
            m_SaveBtn.style.opacity = enabled ? 1f : DISABLED_OPACITY;
        }

        void IPartyMenuUI.SetLocationName(ulong keyHash)
        {
            m_LocationName.Hash = keyHash;

            Localise();
        }

        void IPartyMenuUI.SetPlayTime(uint seconds, bool colonLit)
        {
            m_PlayTimeLabel.text = ClockText.PlayTime(seconds, colonLit);
        }

        void IPartyMenuUI.SetCountdown(uint seconds, bool colonLit)
        {
            m_PlayTimeLabel.text = ClockText.Countdown(seconds, colonLit);
        }

        private void OnConfigChosen()
        {
            m_OnConfigPressed?.Invoke();
        }

        private void OnSaveChosen()
        {
            m_OnSavePressed?.Invoke();
        }

        private void OnLoadChosen()
        {
            m_OnLoadPressed?.Invoke();
        }
    }
}