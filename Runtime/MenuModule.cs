using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        private readonly ICoreModule   m_CoreModule;
        private readonly IDIResolver   m_DIResolver;
        private readonly ISfxPlayer    m_SfxPlayer;
        private readonly IMenuModule   m_MenuModule;
        private readonly Stack<IMenu>  m_Menus;
        private readonly VisualElement m_UIContainer;

        public MenuModule(ICoreModule coreModule, IDIResolver diResolver, ISfxPlayer sfxPlayer)
        {
            m_CoreModule = coreModule;
            m_DIResolver = diResolver;
            m_SfxPlayer  = sfxPlayer;
            m_MenuModule = this;
            m_Menus      = new Stack<IMenu>();

            UIDocument uIDocument = Object.FindAnyObjectByType<UIDocument>();
            m_UIContainer = uIDocument.rootVisualElement;
        }

        Task IModule.OnEnterAsync(IModuleArgs args)
        {
            IMenuModuleArgs menuArgs = (IMenuModuleArgs)args;

            return m_MenuModule.PushMenu(menuArgs);
        }

        Task IModule.OnExitAsync()
        {
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
                await m_MenuModule.ReturnToPreviousModuleAsync();
            }
        }

        void IMenuModule.PlaySfx(int id)
        {
            m_SfxPlayer.Play(id);
        }

        Task IMenuModule.ReturnToPreviousModuleAsync()
        {
            //TODO: get info from save map (module and args for module e.g. IFieldModule, Field 0)

            // return m_CoreModule.LoadModuleAsync(typeof(saveInfo.CurrentModule), saveInfo.CurrentModuleArgs);

            return Task.CompletedTask;
        }
    }
}