using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Core.Rendering;
using RPGFramework.Core.SharedTypes;
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
        private readonly IScreenFadeService m_ScreenFadeService;
        private readonly IMenuArgsStore     m_MenuArgsStore;
        private readonly IMenuTypeProvider  m_MenuTypeProvider;
        private readonly IModuleScene       m_ModuleScene;
        private readonly IMenuModule        m_MenuModule;
        private readonly Stack<IMenu>       m_Menus;
        private readonly VisualElement      m_UIContainer;

        private byte         m_RequestedOutcome;
        private InputAdapter m_InputAdapter;

        public MenuModule(ICoreModule        coreModule,
                          IDIResolver        diResolver,
                          IScreenFadeService screenFadeService,
                          IMenuArgsStore     menuArgsStore,
                          IMenuTypeProvider  menuTypeProvider,
                          IModuleScene       moduleScene)
        {
            m_CoreModule        = coreModule;
            m_DIResolver        = diResolver;
            m_ScreenFadeService = screenFadeService;
            m_MenuArgsStore     = menuArgsStore;
            m_MenuTypeProvider  = menuTypeProvider;
            m_ModuleScene       = moduleScene;
            m_MenuModule        = this;
            m_Menus             = new Stack<IMenu>();

            UIDocument uIDocument = m_ModuleScene.Find<UIDocument>();
            m_UIContainer = uIDocument.rootVisualElement;
        }

        async Task IModule.OnEnterAsync(byte entry)
        {
            await m_ScreenFadeService.FadeOutAsync(true);

            m_InputAdapter = m_ModuleScene.Find<InputAdapter>();
            m_DIResolver.InjectInto(m_InputAdapter);

            MenuArgs args = m_MenuArgsStore.Args;

            await m_MenuModule.PushMenu((MenuType)args.MenuId);
        }

        async Task IModule.OnExitAsync()
        {
            m_InputAdapter.Disable();

            await m_ScreenFadeService.FadeOutAsync();

            while (m_Menus.Count > 0)
            {
                IMenu menu = m_Menus.Pop();
                await menu.OnExitAsync();
            }
        }

        Task IModule.OnSuspendAsync()
        {
            m_InputAdapter.Disable();

            return Task.CompletedTask;
        }

        Task IModule.OnResumeAsync()
        {
            m_InputAdapter.Enable();

            return Task.CompletedTask;
        }

        // Every menu change fades the screen out, swaps the menus and fades back in, with no input while it does.
        async Task IMenuModule.PushMenu(MenuType menuType)
        {
            m_InputAdapter.Disable();

            await m_ScreenFadeService.FadeOutAsync();

            Type  menuToPushType = m_MenuTypeProvider.GetType(menuType);
            IMenu newMenu        = (IMenu)m_DIResolver.Resolve(menuToPushType);

            if (m_Menus.TryPeek(out IMenu menu))
            {
                await menu.OnSuspendAsync(newMenu.HidePreviousUIOnSuspend);
            }

            m_Menus.Push(newMenu);

            await newMenu.OnEnterAsync(m_UIContainer);
            await m_ScreenFadeService.FadeInAsync();

            m_InputAdapter.Enable();
        }

        async Task IMenuModule.PopMenu()
        {
            m_InputAdapter.Disable();

            await m_ScreenFadeService.FadeOutAsync();

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

            await m_ScreenFadeService.FadeInAsync();

            m_InputAdapter.Enable();
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