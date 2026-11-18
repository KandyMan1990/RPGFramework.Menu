using System;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Localisation;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class BeginMenuUI : MenuUI<IBeginMenuUI>, IBeginMenuUI
#if ENABLE_RPG_FRAMEWORK_DEBUG
        , IDebugBeginMenuUI
#endif
    {
        event Action IBeginMenuUI.OnNewGamePressed
        {
            add => m_OnNewGamePressed += value;
            remove => m_OnNewGamePressed -= value;
        }

        event Action IBeginMenuUI.OnLoadGamePressed
        {
            add => m_OnLoadGamePressed += value;
            remove => m_OnLoadGamePressed -= value;
        }

        event Action IBeginMenuUI.OnConfigPressed
        {
            add => m_OnConfigPressed += value;
            remove => m_OnConfigPressed -= value;
        }

        event Action IBeginMenuUI.OnQuitPressed
        {
            add => m_OnQuitPressed += value;
            remove => m_OnQuitPressed -= value;
        }

#if ENABLE_RPG_FRAMEWORK_DEBUG
        event Action IDebugBeginMenuUI.OnDebugRoomPressed
        {
            add => m_OnDebugRoomPressed += value;
            remove => m_OnDebugRoomPressed -= value;
        }

        private event Action m_OnDebugRoomPressed;

        private Label m_DebugRoomBtn;
#endif

        private event Action m_OnNewGamePressed;
        private event Action m_OnLoadGamePressed;
        private event Action m_OnConfigPressed;
        private event Action m_OnQuitPressed;

        private VisualElement m_NewGameBtn;
        private VisualElement m_LoadGameBtn;
        private VisualElement m_ConfigBtn;
        private VisualElement m_QuitBtn;
        private Label         m_VersionLabel;

        protected override VisualElement GetDefaultFocusedElement() => m_NewGameBtn;

        public BeginMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_NewGameBtn   = m_UIInstance.Q("NewGameBtn");
            m_LoadGameBtn  = m_UIInstance.Q("LoadGameBtn");
            m_ConfigBtn    = m_UIInstance.Q("ConfigBtn");
            m_QuitBtn      = m_UIInstance.Q("QuitBtn");
            m_VersionLabel = m_UIInstance.Q<Label>("VersionLabel");

            m_VersionLabel.text = $"v{Application.version}";

#if ENABLE_RPG_FRAMEWORK_DEBUG
            m_DebugRoomBtn      = new Label();
            m_DebugRoomBtn.name = "DebugRoomBtn";
            m_DebugRoomBtn.text = "Debug Room";
            m_DebugRoomBtn.AddToClassList("rpg-ui-label");
            m_QuitBtn.parent.Insert(m_QuitBtn.parent.IndexOf(m_QuitBtn), m_DebugRoomBtn);

            m_Selection.RegisterChosen(m_DebugRoomBtn, OnDebugRoomChosen);
            m_Selection.LinkColumn(new[] { m_NewGameBtn, m_LoadGameBtn, m_ConfigBtn, m_DebugRoomBtn, m_QuitBtn }, true);
#else
            m_Selection.LinkColumn(new[] { m_NewGameBtn, m_LoadGameBtn, m_ConfigBtn, m_QuitBtn }, true);
#endif

            m_Selection.RegisterChosen(m_NewGameBtn, OnNewGameChosen);
            m_Selection.RegisterChosen(m_LoadGameBtn, OnLoadGameChosen);
            m_Selection.RegisterChosen(m_ConfigBtn, OnConfigChosen);
            m_Selection.RegisterChosen(m_QuitBtn, OnQuitChosen);
        }

        private void OnNewGameChosen()
        {
            m_OnNewGamePressed?.Invoke();
        }

        private void OnLoadGameChosen()
        {
            m_OnLoadGamePressed?.Invoke();
        }

        private void OnConfigChosen()
        {
            m_OnConfigPressed?.Invoke();
        }

#if ENABLE_RPG_FRAMEWORK_DEBUG
        private void OnDebugRoomChosen()
        {
            m_OnDebugRoomPressed?.Invoke();
        }
#endif

        private void OnQuitChosen()
        {
            m_OnQuitPressed?.Invoke();
        }
    }
}
