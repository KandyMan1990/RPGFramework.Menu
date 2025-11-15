using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreMenuModule m_CoreModule;
        private readonly IMenuModule     m_MenuModule;
        private readonly Stack<IMenu>    m_Menus;
        private readonly IMenuUIProvider m_UIProvider;

        public MenuModule(ICoreMenuModule coreModule, IMenuUIProvider uiProvider)
        {
            m_CoreModule = coreModule;
            m_UIProvider = uiProvider;
            m_MenuModule = this;
            m_Menus      = new Stack<IMenu>();
        }

        Task IModule.OnEnterAsync(IModuleArgs args)
        {
            IMenuModuleArgs menuArgs = (IMenuModuleArgs)args;

            return m_MenuModule.PushMenu(menuArgs);
        }

        Task IModule.OnExitAsync()
        {
            m_CoreModule.ResetModule<MenuModule>();
            return Task.CompletedTask;
        }

        async Task IMenuModule.PushMenu(IMenuModuleArgs menuModuleArgs)
        {
            IMenu newMenu = (IMenu)m_CoreModule.GetInstance(menuModuleArgs.MenuType);

            if (m_Menus.TryPeek(out IMenu menu))
            {
                await menu.OnSuspendAsync();
            }

            m_Menus.Push(newMenu);

            await newMenu.OnEnterAsync();
        }

        async Task IMenuModule.PopMenu()
        {
            IMenu menu = m_Menus.Pop();
            await menu.OnExitAsync();

            if (m_Menus.TryPeek(out IMenu newMenu))
            {
                await newMenu.OnResumeAsync();
            }
            else
            {
                // TODO: close menu and return to previous module
            }
        }
    }
}