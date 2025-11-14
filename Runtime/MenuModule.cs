using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreMenuModule m_CoreModule;
        private readonly IMenuModule     m_MenuModule;

        private Stack<IMenu> m_Menus;

        public MenuModule(ICoreMenuModule coreModule)
        {
            m_CoreModule = coreModule;
            m_MenuModule = this;
            m_Menus      = new Stack<IMenu>();
        }

        async Task IModule.OnEnterAsync(IModuleArgs args)
        {
            await SceneManager.LoadSceneAsync(nameof(MenuModule));

            IMenuModuleArgs menuArgs = (IMenuModuleArgs)args;

            await m_MenuModule.PushMenu(menuArgs);
        }

        Task IModule.OnExitAsync()
        {
            m_CoreModule.ResetModule<MenuModule>();
            return Task.CompletedTask;
        }

        Task IMenuModule.PushMenu(IMenuModuleArgs menuModuleArgs)
        {
            IMenu menu = (IMenu)m_CoreModule.GetInstance(menuModuleArgs.MenuType);

            m_Menus.Push(menu);

            return menu.OnEnterAsync();
        }

        Task IMenuModule.PopMenu()
        {
            IMenu menu = m_Menus.Pop();

            return menu.OnExitAsync();
        }
    }
}