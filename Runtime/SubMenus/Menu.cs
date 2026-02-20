using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Menu.SharedTypes;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public abstract class Menu<TMenuUI> : IInputContext, IMenu where TMenuUI : IMenuUI
    {
        bool IMenu.HidePreviousUiOnSuspend => m_HidePreviousUiOnSuspend;

        protected abstract bool m_HidePreviousUiOnSuspend { get; }

        protected readonly TMenuUI            m_MenuUI;
        protected readonly IInputRouter       m_InputRouter;
        protected readonly IMenuModule        m_MenuModule;
        protected readonly IAudioIntentPlayer m_AudioIntentPlayer;

        protected Menu(TMenuUI            menuUI,
                       IInputRouter       inputRouter,
                       IMenuModule        menuModule,
                       IAudioIntentPlayer audioIntentPlayer)
        {
            m_MenuUI            = menuUI;
            m_InputRouter       = inputRouter;
            m_MenuModule        = menuModule;
            m_AudioIntentPlayer = audioIntentPlayer;
        }

        async Task IMenu.OnEnterAsync(VisualElement parent, Dictionary<string, object> args)
        {
            await OnEnterAsync(args);

            await m_MenuUI.OnEnterAsync(parent, args);

            RegisterCallbacks();

            m_InputRouter.Push(this);

            await OnEnterComplete();
        }

        async Task IMenu.OnSuspendAsync(bool hideUi)
        {
            UnregisterCallbacks();

            await m_MenuUI.OnSuspendAsync(hideUi);

            await OnSuspendAsync(hideUi);
        }

        async Task IMenu.OnResumeAsync()
        {
            await OnResumeAsync();

            await m_MenuUI.OnResumeAsync();

            RegisterCallbacks();
        }

        async Task IMenu.OnExitAsync()
        {
            m_InputRouter.Pop(this);

            UnregisterCallbacks();

            await m_MenuUI.OnExitAsync();

            await OnExitAsync();
        }

        bool IInputContext.Handle(ControlSlot slot)
        {
            return HandleControl(slot);
        }

        void IInputContext.HandleMove(Vector2 move)
        {
            // noop
        }

        protected virtual Task OnEnterAsync(Dictionary<string, object> args)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnEnterComplete()
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

        protected abstract bool HandleControl(ControlSlot slot);
    }
}