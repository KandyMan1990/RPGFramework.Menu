using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Memory;
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

        private readonly IVariableMap   m_VariableMap;
        private readonly IMemoryService m_MemoryService;

        // Whether each of the UI's entries is offered, as it was last greyed: what a choice is judged by is what shows.
        private bool[] m_Offered;

        protected Menu(TMenuUI            menuUI,
                       IInputRouter       inputRouter,
                       IMenuModule        menuModule,
                       ISfxPlayer         sfxPlayer,
                       ISystemSoundConfig systemSounds,
                       IVariableMap       variableMap,
                       IMemoryService     memoryService)
        {
            m_MenuUI        = menuUI;
            m_InputRouter   = inputRouter;
            m_MenuModule    = menuModule;
            m_SfxPlayer     = sfxPlayer;
            m_SystemSounds  = systemSounds;
            m_VariableMap   = variableMap;
            m_MemoryService = memoryService;
            m_Offered       = Array.Empty<bool>();

            m_MenuUI.OnCursorMoved  += OnCursorMoved;
            m_MenuUI.OnEntriesFound += GreyEntries;
            m_MenuUI.OnEntryChosen  += OnEntryChosen;
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

            GreyEntries();

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

        // As the menu opens and each time it resumes, as what greys an entry may have changed in a menu opened over it.
        private void GreyEntries()
        {
            IReadOnlyList<MenuChoice> entries = m_MenuUI.Entries;

            if (m_Offered.Length != entries.Count)
            {
                m_Offered = new bool[entries.Count];
            }

            for (int i = 0; i < entries.Count; i++)
            {
                MenuChoice entry   = entries[i];
                bool       offered = (entry.Opens == 0 || CanOpen(entry.Opens)) &&
                                     MenuEntryConditions.Holds(entry.When, m_VariableMap, m_MemoryService);

                m_Offered[i] = offered;
                m_MenuUI.SetEntryGreyed(i, !offered);
            }
        }

        private void OnEntryChosen(int entry)
        {
            if (!m_Offered[entry])
            {
                PlaySound(m_SystemSounds.Error);
                return;
            }

            MenuChoice choice = m_MenuUI.Entries[entry];

            if (choice.Opens != 0)
            {
                PlaySound(m_SystemSounds.Confirm);
                m_MenuModule.PushMenu(choice.Opens).FireAndForget();
            }
            else if (choice.Command != 0)
            {
                OnCommand(choice.Command);
            }
        }

        /// <summary>
        /// Whether an entry opening the menu named by <paramref name="menuId" /> is offered now, besides any variable it
        /// names: the menu's own conditions, such as the party menu's Save needing the save flag. Asked as the menu opens
        /// and as it resumes.
        /// </summary>
        protected virtual bool CanOpen(ulong menuId)
        {
            return true;
        }

        /// <summary>
        /// An offered entry naming <paramref name="command" />, the hash of its name, was chosen: work the menu carries out
        /// itself, playing its own sound, as the title's New Game and Quit.
        /// </summary>
        protected virtual void OnCommand(ulong command)
        {
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
