using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Core.SharedTypes;
using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreModule   m_CoreModule;
        private readonly IDIResolver   m_DIResolver;
        private readonly IMenuModule   m_MenuModule;
        private readonly Stack<IMenu>  m_Menus;
        private readonly VisualElement m_UIContainer;

        private InputAdapter m_InputAdapter;

        public MenuModule(ICoreModule coreModule, IDIResolver diResolver)
        {
            m_CoreModule = coreModule;
            m_DIResolver = diResolver;
            m_MenuModule = this;
            m_Menus      = new Stack<IMenu>();

            UIDocument uIDocument = Object.FindAnyObjectByType<UIDocument>();
            m_UIContainer = uIDocument.rootVisualElement;
        }

        Task IModule.OnEnterAsync(IModuleArgs args)
        {
            m_InputAdapter = Object.FindFirstObjectByType<InputAdapter>();
            m_DIResolver.InjectInto(m_InputAdapter);
            m_InputAdapter.Enable();

            IMenuModuleArgs menuArgs = (IMenuModuleArgs)args;

            return m_MenuModule.PushMenu(menuArgs);
        }

        Task IModule.OnExitAsync()
        {
            m_InputAdapter.Disable();

            m_CoreModule.ResetModule<IMenuModule, MenuModule>();

            return Task.CompletedTask;
        }

        async Task IMenuModule.PushMenu(IMenuModuleArgs menuModuleArgs)
        {
            IMenu newMenu = (IMenu)m_DIResolver.Resolve(menuModuleArgs.MenuType);

            if (m_Menus.TryPeek(out IMenu menu))
            {
                await menu.OnSuspendAsync(newMenu.HidePreviousUiOnSuspend);
            }

            m_Menus.Push(newMenu);

            await newMenu.OnEnterAsync(m_UIContainer);
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
                m_CoreModule.ResumeModuleAsync().FireAndForget();
            }
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