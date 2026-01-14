using System;
using RPGFramework.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class LanguageMenuUI : MenuUI<ILanguageMenuUI>, ILanguageMenuUI
    {
        event Action<int> ILanguageMenuUI.OnLanguageChanged
        {
            add => m_OnLanguageChanged += value;
            remove => m_OnLanguageChanged -= value;
        }

        protected override VisualElement GetDefaultFocusedElement() => m_LanguageBtn;

        private event Action<int> m_OnLanguageChanged;

        private Label       m_TitleLabel;
        private RPGUIButton m_LanguageBtn;
        private Label       m_LanguageLabel;

        public LanguageMenuUI(ILanguageMenuLocalisationArgs localisationArgs,
                              IMenuUIProvider               uiProvider,
                              IGenericAudioIdProvider       audioIdProvider,
                              ILocalisationService          localisationService) : base(localisationArgs, uiProvider, audioIdProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_TitleLabel    = m_UIInstance.Q<Label>("TitleLabel");
            m_LanguageBtn   = m_UIInstance.Q<RPGUIButton>("LanguageBtn");
            m_LanguageLabel = m_UIInstance.Q<Label>("LanguageLabel");
        }

        protected override void LocaliseUI()
        {
            ILanguageMenuLocalisationArgs args = (ILanguageMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text    = m_LocalisationService.Get(args.ScreenTitle);
            m_LanguageBtn.text   = m_LocalisationService.Get(args.Language);
            m_LanguageLabel.text = m_LocalisationService.Get(args.Language);
        }

        protected override void RegisterCallbacks()
        {
            UIToolkitInputUtility.RegisterButtonCallbacks(m_LanguageBtn, OnLanguageBtnNavigate, null, null, OnBtnFocus);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_LanguageBtn, OnLanguageBtnNavigate, null, null, OnBtnFocus);
        }
        
        void ILanguageMenuUI.RefreshLocalisation()
        {
            LocaliseUI();
        }

        private void OnLanguageBtnNavigate(NavigationMoveEvent evt)
        {
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
    }
}