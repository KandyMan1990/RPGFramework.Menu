using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Core.Transitions;
using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SharedTypes.Stores;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreModule        m_CoreModule;
        private readonly IDIResolver        m_DIResolver;
        private readonly ITransitionService m_Transitions;
        private readonly IMenuArgsStore     m_MenuArgsStore;
        private readonly IMenuTypeProvider  m_MenuTypeProvider;
        private readonly IModuleScene       m_ModuleScene;
        private readonly IMenuModule        m_MenuModule;
        private readonly Stack<IMenu>       m_Menus;
        private readonly VisualElement      m_UIContainer;

        private byte         m_RequestedOutcome;
        private InputAdapter m_InputAdapter;
        private bool         m_Entering;

        public MenuModule(ICoreModule        coreModule,
                          IDIResolver        diResolver,
                          ITransitionService transitions,
                          IMenuArgsStore     menuArgsStore,
                          IMenuTypeProvider  menuTypeProvider,
                          IModuleScene       moduleScene)
        {
            m_CoreModule        = coreModule;
            m_DIResolver        = diResolver;
            m_Transitions       = transitions;
            m_MenuArgsStore     = menuArgsStore;
            m_MenuTypeProvider  = menuTypeProvider;
            m_ModuleScene       = moduleScene;
            m_MenuModule        = this;
            m_Menus             = new Stack<IMenu>();
            m_Entering          = false;

            UIDocument uIDocument = m_ModuleScene.Find<UIDocument>();
            m_UIContainer = uIDocument.rootVisualElement;
        }

        // Core reveals the module once it has entered, so the menus pushed as it enters — the title's language menu on a
        // first launch among them — open under the cover rather than revealing themselves.
        async Task IModule.OnEnterAsync(byte entry)
        {
            m_InputAdapter = m_ModuleScene.Find<InputAdapter>();
            m_DIResolver.InjectInto(m_InputAdapter);

            MenuArgs args = m_MenuArgsStore.Args;

            m_Entering = true;

            await m_MenuModule.PushMenu((MenuType)args.MenuId);

            m_Entering = false;
        }

        async Task IModule.OnExitAsync()
        {
            m_InputAdapter.Disable();
            SetTopMenuInputEnabled(false);

            while (m_Menus.Count > 0)
            {
                IMenu menu = m_Menus.Pop();
                await menu.OnExitAsync();
            }
        }

        Task IModule.OnSuspendAsync()
        {
            m_InputAdapter.Disable();
            SetTopMenuInputEnabled(false);

            return Task.CompletedTask;
        }

        Task IModule.OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        void IModule.OnRevealed()
        {
            SetTopMenuInputEnabled(true);
            m_InputAdapter.Enable();
        }

        // Every menu change fades the screen out, swaps the menus and fades back in, with no input from the moment it
        // starts until it has faded in: a press during the fade would start another change.
        async Task IMenuModule.PushMenu(MenuType menuType)
        {
            m_InputAdapter.Disable();
            SetTopMenuInputEnabled(false);

            await m_Transitions.CoverAsync();

            Type  menuToPushType = m_MenuTypeProvider.GetType(menuType);
            IMenu newMenu        = (IMenu)m_DIResolver.Resolve(menuToPushType);

            if (m_Menus.TryPeek(out IMenu menu))
            {
                await menu.OnSuspendAsync(newMenu.HidePreviousUIOnSuspend);
            }

            m_Menus.Push(newMenu);

            await newMenu.OnEnterAsync(m_UIContainer);

            if (m_Entering)
            {
                return;
            }

            await m_Transitions.RevealAsync();

            SetTopMenuInputEnabled(true);
            m_InputAdapter.Enable();
        }

        async Task IMenuModule.PopMenu()
        {
            m_InputAdapter.Disable();
            SetTopMenuInputEnabled(false);

            await m_Transitions.CoverAsync();

            IMenu menu = m_Menus.Pop();
            await menu.OnExitAsync();

            if (!m_Menus.TryPeek(out IMenu newMenu))
            {
                LeaveModule(m_RequestedOutcome != 0 ? m_RequestedOutcome : MenuOutcomes.CLOSED);
                return;
            }

            await newMenu.OnResumeAsync();

            // A menu leaving the module, as a new game or a load does, leaves with the screen already out.
            if (m_RequestedOutcome != 0)
            {
                LeaveModule(m_RequestedOutcome);
                return;
            }

            await m_Transitions.RevealAsync();

            SetTopMenuInputEnabled(true);
            m_InputAdapter.Enable();
        }

        // The top when the change ends, not the menu it pushed: a menu may open another as it enters, as the title does
        // its language menu on a first launch.
        private void SetTopMenuInputEnabled(bool enabled)
        {
            if (m_Menus.TryPeek(out IMenu top))
            {
                top.SetInputEnabled(enabled);
            }
        }

        void IMenuModule.RequestModuleChange(byte outcome)
        {
            m_RequestedOutcome = outcome;
        }

        private void LeaveModule(byte outcome)
        {
            m_RequestedOutcome = 0;

            m_CoreModule.RequestModuleChangeAsync(outcome).FireAndForget();
        }

        bool IMenuModule.IsMenuInStack<T>()
        {
            Type type = typeof(T);
            if (type.GetInterface(nameof(IMenu)) == null)
            {
                return false;
            }

            foreach (IMenu menu in m_Menus)
            {
                if (menu is T)
                {
                    return true;
                }
            }

            return false;
        }
    }
}