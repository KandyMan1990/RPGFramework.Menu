using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public abstract class Menu<TMenuUI> : IMenu where TMenuUI : IMenuUI
    {
        bool IMenu.HidePreviousUiOnSuspend => m_HidePreviousUiOnSuspend;
        
        protected abstract bool m_HidePreviousUiOnSuspend { get; }
        
        protected readonly TMenuUI m_MenuUI;
        
        protected Menu(TMenuUI menuUI)
        {
            m_MenuUI = menuUI;
        }
        
        async Task IMenu.OnEnterAsync(VisualElement parent, Dictionary<string, object> args)
        {
            await m_MenuUI.OnEnterAsync(parent, args);

            await OnEnterAsync(args);

            RegisterCallbacks();
        }

        async Task IMenu.OnSuspendAsync(bool hideUi)
        {
            UnregisterCallbacks();

            await m_MenuUI.OnSuspendAsync(hideUi);

            await OnSuspendAsync(hideUi);
        }

        async Task IMenu.OnResumeAsync()
        {
            await m_MenuUI.OnResumeAsync();

            RegisterCallbacks();

            await OnResumeAsync();
        }

        async Task IMenu.OnExitAsync()
        {
            UnregisterCallbacks();
            
            await m_MenuUI.OnExitAsync();

            await OnExitAsync();
        }

        protected virtual Task OnEnterAsync(Dictionary<string, object> args)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnSuspendAsync(bool hideUi)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnExitAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual void RegisterCallbacks()
        {
            
        }

        protected virtual void UnregisterCallbacks()
        {
            
        }
    }
}