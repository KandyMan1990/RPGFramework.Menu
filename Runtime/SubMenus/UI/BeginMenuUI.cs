using System;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.UI;
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

        private RPGUIButton m_DebugRoomBtn;
#endif

        private event Action       m_OnNewGamePressed;
        private event Action       m_OnLoadGamePressed;
        private event Action m_OnQuitPressed;

        private Label       m_TitleLabel;
        private RPGUIButton m_NewGameBtn;
        private RPGUIButton m_LoadGameBtn;
        private RPGUIButton m_QuitBtn;
        private Label       m_VersionLabel;

        protected override VisualElement GetDefaultFocusedElement() => m_NewGameBtn;

        public BeginMenuUI(IBeginMenuLocalisationArgs localisationArgs,
                           IMenuUIProvider            uiProvider,
                           ILocalisationService       localisationService) : base(localisationArgs, uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_TitleLabel   = m_UIInstance.Q<Label>("TitleLabel");
            m_NewGameBtn   = m_UIInstance.Q<RPGUIButton>("NewGameBtn");
            m_LoadGameBtn  = m_UIInstance.Q<RPGUIButton>("LoadGameBtn");
            m_QuitBtn      = m_UIInstance.Q<RPGUIButton>("QuitBtn");
            m_VersionLabel = m_UIInstance.Q<Label>("VersionLabel");

            m_VersionLabel.text = $"v{Application.version}";

#if ENABLE_RPG_FRAMEWORK_DEBUG
            m_DebugRoomBtn      = new RPGUIButton();
            m_DebugRoomBtn.name = "DebugRoomBtn";
            m_DebugRoomBtn.text = "Debug Room";
            m_QuitBtn.parent.Insert(m_QuitBtn.parent.IndexOf(m_QuitBtn), m_DebugRoomBtn);

            m_DebugRoomBtn.Chosen += OnDebugRoomChosen;

            RPGUIButton.LinkColumn(new RPGUIButton[] { m_NewGameBtn, m_LoadGameBtn, m_DebugRoomBtn, m_QuitBtn }, true);
#else
            RPGUIButton.LinkColumn(new RPGUIButton[] { m_NewGameBtn, m_LoadGameBtn, m_QuitBtn }, true);
#endif

            m_NewGameBtn.Chosen  += OnNewGameChosen;
            m_LoadGameBtn.Chosen += OnLoadGameChosen;
            m_QuitBtn.Chosen     += OnQuitChosen;
        }

        protected override void LocaliseUI()
        {
            IBeginMenuLocalisationArgs args = (IBeginMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text  = m_LocalisationService.Get(args.GameTitle);
            m_NewGameBtn.text  = m_LocalisationService.Get(args.NewGame);
            m_LoadGameBtn.text = m_LocalisationService.Get(args.LoadGame);
            m_QuitBtn.text     = m_LocalisationService.Get(args.QuitGame);
        }

        private void OnNewGameChosen()
        {
            m_OnNewGamePressed?.Invoke();
        }

        private void OnLoadGameChosen()
        {
            m_OnLoadGamePressed?.Invoke();
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