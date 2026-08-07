using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Input;
using RPGFramework.Core.Rendering;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Core.Store;
using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Providers;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreModule        m_CoreModule;
        private readonly IDIResolver        m_DIResolver;
        private readonly IScreenFadeService m_ScreenFadeService;
        private readonly IMenuArgsProvider  m_MenuArgsProvider;
        private readonly IMenuTypeProvider  m_MenuTypeProvider;
        private readonly IChangeModuleStore m_ChangeModuleStore;
        private readonly IResumeModuleStore m_ResumeModuleStore;
        private readonly IMenuModule        m_MenuModule;
        private readonly Stack<IMenu>       m_Menus;
        private readonly VisualElement      m_UIContainer;

        private InputAdapter m_InputAdapter;

        public MenuModule(ICoreModule        coreModule,
                          IDIResolver        diResolver,
                          IScreenFadeService screenFadeService,
                          IMenuArgsProvider  menuArgsProvider,
                          IMenuTypeProvider  menuTypeProvider,
                          IChangeModuleStore changeModuleStore,
                          IResumeModuleStore resumeModuleStore)
        {
            m_CoreModule        = coreModule;
            m_DIResolver        = diResolver;
            m_ScreenFadeService = screenFadeService;
            m_MenuArgsProvider  = menuArgsProvider;
            m_MenuTypeProvider  = menuTypeProvider;
            m_ChangeModuleStore = changeModuleStore;
            m_ResumeModuleStore = resumeModuleStore;
            m_MenuModule        = this;
            m_Menus             = new Stack<IMenu>();

            UIDocument uIDocument = Object.FindAnyObjectByType<UIDocument>();
            m_UIContainer = uIDocument.rootVisualElement;
        }

        async Task IModule.OnEnterAsync()
        {
            await m_ScreenFadeService.FadeOutAsync(true);

            m_InputAdapter = Object.FindAnyObjectByType<InputAdapter>();
            m_DIResolver.InjectInto(m_InputAdapter);

            MenuArgs args = m_MenuArgsProvider.Get;

            await m_MenuModule.PushMenu((MenuType)args.MenuId);
            await m_ScreenFadeService.FadeInAsync();

            m_InputAdapter.Enable();
        }

        async Task IModule.OnExitAsync()
        {
            m_InputAdapter.Disable();

            while (m_Menus.Count > 0)
            {
                IMenu menu = m_Menus.Pop();
                await menu.OnExitAsync();
            }

            await m_ScreenFadeService.FadeOutAsync();

            m_CoreModule.ResetModule<IMenuModule, MenuModule>();
        }

        async Task IMenuModule.PushMenu(MenuType menuType)
        {
            Type  menuToPushType = m_MenuTypeProvider.GetType(menuType);
            IMenu newMenu        = (IMenu)m_DIResolver.Resolve(menuToPushType);

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
                byte moduleId = m_ResumeModuleStore.GetModuleId;
                m_ChangeModuleStore.SetModuleId(moduleId);

                m_CoreModule.RequestModuleChangeAsync().FireAndForget();
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