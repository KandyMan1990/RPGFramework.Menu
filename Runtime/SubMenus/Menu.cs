using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Menu.SharedTypes;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public abstract class Menu<TMenuUI> : IInputContext, IMenu where TMenuUI : IMenuUI
    {
        bool IMenu.HidePreviousUIOnSuspend => m_HidePreviousUIOnSuspend;

        protected abstract bool m_HidePreviousUIOnSuspend { get; }

        protected readonly TMenuUI            m_MenuUI;
        protected readonly IInputRouter       m_InputRouter;
        protected readonly IMenuModule        m_MenuModule;
        protected readonly ISfxPlayer         m_SfxPlayer;
        protected readonly ISystemSoundConfig m_SystemSounds;

        protected Menu(TMenuUI            menuUI,
                       IInputRouter       inputRouter,
                       IMenuModule        menuModule,
                       ISfxPlayer         sfxPlayer,
                       ISystemSoundConfig systemSounds)
        {
            m_MenuUI       = menuUI;
            m_InputRouter  = inputRouter;
            m_MenuModule   = menuModule;
            m_SfxPlayer    = sfxPlayer;
            m_SystemSounds = systemSounds;

            m_MenuUI.OnCursorMoved += OnCursorMoved;
        }

        async Task IMenu.OnEnterAsync(VisualElement parent, ulong menuId, Dictionary<string, object> args)
        {
            await OnEnterAsync(args);

            await m_MenuUI.OnEnterAsync(parent, menuId, args);

            m_InputRouter.Push(this);

            await OnEnterComplete();
        }

        async Task IMenu.OnSuspendAsync(bool hideUI)
        {
            await m_MenuUI.OnSuspendAsync(hideUI);

            await OnSuspendAsync(hideUI);
        }

        async Task IMenu.OnResumeAsync()
        {
            await OnResumeAsync();

            await m_MenuUI.OnResumeAsync();
        }

        async Task IMenu.OnExitAsync()
        {
            m_InputRouter.Pop(this);

            SetInputEnabled(false);

            await m_MenuUI.OnExitAsync();

            await OnExitAsync();
        }

        void IMenu.SetInputEnabled(bool enabled)
        {
            SetInputEnabled(enabled);
        }

        protected void SetInputEnabled(bool enabled)
        {
            m_MenuUI.SetInputEnabled(enabled);
        }

        bool IInputContext.Handle(ulong action)
        {
            return HandleAction(action);
        }

        bool IInputContext.TakesMovement => true;

        private void OnCursorMoved()
        {
            PlaySound(m_SystemSounds.Cursor);
        }

        /// <summary>Plays a sound, one of <see cref="m_SystemSounds" /> or the menu's own, unless it is none.</summary>
        protected void PlaySound(ulong sound)
        {
            if (sound != 0)
            {
                m_SfxPlayer.PlayAsync(sound).FireAndForget();
            }
        }

        protected virtual Task OnEnterAsync(Dictionary<string, object> args)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnEnterComplete()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnSuspendAsync(bool hideUI)
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

        /// <summary>A button action pressed, named by the hash of its name; true if this menu took it.</summary>
        protected abstract bool HandleAction(ulong action);
    }
}
