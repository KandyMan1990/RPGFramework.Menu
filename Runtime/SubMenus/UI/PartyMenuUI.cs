using System;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public sealed class PartyMenuUI : MenuUI<IPartyMenuUI>, IPartyMenuUI
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
        private Label       m_LocationLabel;
        private Label       m_TimeLabel;
        private Label       m_PlayTimeLabel;

        private ulong m_LocationName;

        protected override VisualElement GetDefaultFocusedElement() => m_ConfigBtn;

        public PartyMenuUI(IPartyMenuLocalisationArgs localisationArgs,
                           IMenuUIProvider            uiProvider,
                           ILocalisationService       localisationService) : base(localisationArgs, uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_ConfigBtn     = m_UIInstance.Q<RPGUIButton>("ConfigBtn");
            m_SaveBtn       = m_UIInstance.Q<RPGUIButton>("SaveBtn");
            m_LoadBtn       = m_UIInstance.Q<RPGUIButton>("LoadBtn");
            m_LocationLabel = m_UIInstance.Q<Label>("LocationLabel");
            m_TimeLabel     = m_UIInstance.Q<Label>("TimeLabel");
            m_PlayTimeLabel = m_UIInstance.Q<Label>("PlayTimeLabel");

            RPGUIButton.LinkColumn(new RPGUIButton[] { m_ConfigBtn, m_SaveBtn, m_LoadBtn }, true);

            m_ConfigBtn.Chosen += OnConfigChosen;
            m_SaveBtn.Chosen   += OnSaveChosen;
            m_LoadBtn.Chosen   += OnLoadChosen;
        }

        protected override void LocaliseUI()
        {
            IPartyMenuLocalisationArgs args = (IPartyMenuLocalisationArgs)m_LocalisationArgs;

            m_ConfigBtn.text = m_LocalisationService.Get(args.Config);
            m_SaveBtn.text   = m_LocalisationService.Get(args.Save);
            m_LoadBtn.text   = m_LocalisationService.Get(args.Load);
            m_TimeLabel.text = m_LocalisationService.Get(args.Time);

            m_LocationLabel.text = m_LocalisationService.TryGet(m_LocationName, out string locationName) ? locationName : string.Empty;
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