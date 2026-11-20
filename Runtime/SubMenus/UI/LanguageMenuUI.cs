using System;
using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class LanguageMenuUI : MenuUI<ILanguageMenuUI>, ILanguageMenuUI
    {
        private const string LANGUAGE_BUTTON = "LanguageBtn";

        event Action<int> ILanguageMenuUI.OnLanguageChanged
        {
            add => m_OnLanguageChanged += value;
            remove => m_OnLanguageChanged -= value;
        }

        protected override VisualElement GetDefaultFocusedElement() => m_LanguageBtn;

        private event Action<int> m_OnLanguageChanged;

        [LayoutElement(LANGUAGE_BUTTON)]
        private VisualElement m_LanguageBtn;

        public LanguageMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_LanguageBtn = m_UIInstance.Q(LANGUAGE_BUTTON);

            m_Selection.LinkColumn(new[] { m_LanguageBtn }, false);
            m_LanguageBtn.RegisterCallback<NavigationMoveEvent>(OnLanguageBtnNavigate);
        }

        void ILanguageMenuUI.RefreshLocalisation()
        {
            Localise();
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
    }
}
