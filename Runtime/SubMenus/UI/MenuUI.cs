using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public abstract class MenuUI<TMenuUI> : IMenuUI where TMenuUI : IMenuUI
    {
        event Action IMenuUI.OnBackButtonPressed
        {
            add => m_OnBackButtonPressed += value;
            remove => m_OnBackButtonPressed -= value;
        }

        event Action IMenuUI.OnCursorMoved
        {
            add => m_OnCursorMoved += value;
            remove => m_OnCursorMoved -= value;
        }

        event Action IMenuUI.OnEntriesFound
        {
            add => m_OnEntriesFound += value;
            remove => m_OnEntriesFound -= value;
        }

        event Action<int> IMenuUI.OnEntryChosen
        {
            add => m_OnEntryChosen += value;
            remove => m_OnEntryChosen -= value;
        }

        IReadOnlyList<MenuChoice> IMenuUI.Entries => m_Choices;

        VisualElement IMenuUI.GetDefaultFocusedElement() => GetDefaultFocusedElement();
        VisualElement IMenuUI.GetLastFocusedElement()    => m_LastFocusedElement;

        private const string GREYED_CLASS    = "rpg-greyed";
        private const string HELP_LINE_CLASS = "rpg-help-line";

        private event Action      m_OnBackButtonPressed;
        private event Action      m_OnCursorMoved;
        private event Action      m_OnEntriesFound;
        private event Action<int> m_OnEntryChosen;

        protected readonly IMenuUIProvider      m_UIProvider;
        protected readonly ILocalisationService m_LocalisationService;

        protected VisualElement m_UIInstance;

        protected VisualElement m_LastFocusedElement;

        /// <summary>The scope the UI's elements are linked in, for the cursor to move between them and be chosen.</summary>
        protected SelectionScope m_Selection;

        private string[]     m_SheetNames;
        private MenuEntry[]  m_Entries;
        private MenuChoice[] m_Choices;
        private Label        m_HelpLine;

        protected MenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
        {
            m_UIProvider          = uiProvider;
            m_LocalisationService = localisationService;
            m_Selection           = null;
            m_SheetNames          = Array.Empty<string>();
            m_Entries             = Array.Empty<MenuEntry>();
            m_Choices             = Array.Empty<MenuChoice>();
            m_HelpLine            = null;
        }

        async Task IMenuUI.OnEnterAsync(VisualElement parent, ulong menuId, Dictionary<string, object> args)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetLayout(menuId);
            uiAsset.CloneTree(parent);

            m_UIInstance = parent[parent.childCount - 1];
            m_Selection  = new SelectionScope(m_UIInstance);

            m_Selection.Moved += OnSelectionMoved;

            AddEntries();
            FindEntries();

            ShowUI(false);

            m_SheetNames = WithHelpSheets(GetSheetNames());

            await m_LocalisationService.LoadNewLocalisationDataAsync(m_SheetNames);

            await OnEnterAsync(args);

            HookupUI();

            m_OnEntriesFound?.Invoke();

            Localise();

            ShowUI(true);

            GetDefaultFocusedElement()?.Focus();

            OnEnterComplete();

            await OnShowAnimationAsync();
        }

        async Task IMenuUI.OnSuspendAsync(bool hideUI)
        {
            m_LastFocusedElement = (VisualElement)m_UIInstance.focusController.focusedElement;

            await OnHideAnimationAsync();

            ShowUI(!hideUI);

            m_LocalisationService.UnloadLocalisationData(m_SheetNames);

            await OnSuspendAsync(hideUI);
        }

        async Task IMenuUI.OnResumeAsync()
        {
            await m_LocalisationService.LoadNewLocalisationDataAsync(m_SheetNames);

            Localise();

            ShowUI(true);

            await OnResumeAsync();

            m_LastFocusedElement?.Focus();
            m_LastFocusedElement = null;

            await OnShowAnimationAsync();
        }

        async Task IMenuUI.OnExitAsync()
        {
            SetInputEnabled(false);

            await OnHideAnimationAsync();

            await OnExitAsync();

            m_UIInstance.RemoveFromHierarchy();
            m_UIInstance = null;
            m_Selection  = null;
            m_Entries    = Array.Empty<MenuEntry>();
            m_Choices    = Array.Empty<MenuChoice>();
            m_HelpLine   = null;
        }

        void IMenuUI.SetInputEnabled(bool enabled)
        {
            SetInputEnabled(enabled);
        }

        void IMenuUI.SetEntryGreyed(int entry, bool greyed)
        {
            m_Entries[entry].EnableInClassList(GREYED_CLASS, greyed);
        }

        private void SetInputEnabled(bool enabled)
        {
            m_Selection.SetInputEnabled(enabled);
        }

        protected virtual Task OnEnterAsync(Dictionary<string, object> args)
        {
            return Task.CompletedTask;
        }

        protected virtual void OnEnterComplete()
        {

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
            m_LocalisationService.UnloadLocalisationData(m_SheetNames);

            return Task.CompletedTask;
        }

        protected virtual void HookupUI()
        {
        }

        /// <summary>
        /// Entries the UI makes in code, put into its layout before the layout's entries are found, as the title adds the
        /// debug room's.
        /// </summary>
        protected virtual void AddEntries()
        {
        }

        /// <summary>
        /// The element the cursor starts on as the menu opens: by default the entry marked to, while it is offered, else
        /// the layout's first entry, or none without one.
        /// </summary>
        protected virtual VisualElement GetDefaultFocusedElement()
        {
            for (int i = 0; i < m_Entries.Length; i++)
            {
                if (m_Entries[i].DefaultFocus && !m_Entries[i].ClassListContains(GREYED_CLASS))
                {
                    return m_Entries[i];
                }
            }

            return m_Entries.Length > 0 ? m_Entries[0] : null;
        }

        /// <summary>
        /// The sheets the menu loads while it is on top: those its layout's localised texts name.
        /// </summary>
        protected virtual string[] GetSheetNames() => new LocalisedTexts(m_UIInstance).SheetNames;

        /// <summary>
        /// Shows the menu's text in the current language: what the code chooses (<see cref="LocaliseUI" />), then every
        /// localised text in its layout, found afresh, as a menu may have added or removed some.
        /// </summary>
        protected void Localise()
        {
            LocaliseUI();

            new LocalisedTexts(m_UIInstance).Show(m_LocalisationService);

            ShowHelp(m_UIInstance.focusController?.focusedElement as VisualElement);
        }

        /// <summary>Text the code chooses: which of the layout's texts shows, or the entry one shows by hash.</summary>
        protected virtual void LocaliseUI()
        {
        }

        protected virtual void ShowUI(bool isShow)
        {
            m_UIInstance.SetEnabledAndVisible(isShow);
        }

        protected void RaiseOnBackButtonPressed()
        {
            m_OnBackButtonPressed?.Invoke();
        }

        private void OnSelectionMoved()
        {
            m_OnCursorMoved?.Invoke();
        }

        // The layout's entries in its order, a wrapping column unless the UI links them otherwise as it is hooked up.
        private void FindEntries()
        {
            m_Entries = m_UIInstance.Query<MenuEntry>().ToList().ToArray();
            m_Choices = new MenuChoice[m_Entries.Length];

            for (int i = 0; i < m_Entries.Length; i++)
            {
                int entry = i;

                m_Choices[i] = m_Entries[i].ToChoice();
                m_Selection.RegisterChosen(m_Entries[i], () => m_OnEntryChosen?.Invoke(entry));
            }

            m_Selection.LinkColumn(m_Entries, true);

            m_HelpLine = m_UIInstance.Q<Label>(className: HELP_LINE_CLASS);

            if (m_HelpLine != null)
            {
                m_UIInstance.RegisterCallback<FocusInEvent>(OnFocusIn);
            }
        }

        // The sheets the entries' help is in, beside the layout's own.
        private string[] WithHelpSheets(string[] sheetNames)
        {
            string[] names = new string[sheetNames.Length + m_Entries.Length];
            int      count = sheetNames.Length;

            Array.Copy(sheetNames, names, count);

            for (int i = 0; i < m_Entries.Length; i++)
            {
                string help = m_Entries[i].Help;

                if (string.IsNullOrEmpty(help))
                {
                    continue;
                }

                string sheet = help.Substring(0, help.IndexOf('/'));

                if (Array.IndexOf(names, sheet, 0, count) < 0)
                {
                    names[count++] = sheet;
                }
            }

            string[] withHelp = new string[count];
            Array.Copy(names, withHelp, count);

            return withHelp;
        }

        private void OnFocusIn(FocusInEvent evt)
        {
            ShowHelp(evt.target as VisualElement);
        }

        private void ShowHelp(VisualElement focused)
        {
            if (m_HelpLine == null)
            {
                return;
            }

            int    entry = Array.IndexOf(m_Entries, focused);
            string help  = entry >= 0 ? m_Entries[entry].Help : null;

            m_HelpLine.text = string.IsNullOrEmpty(help) ? string.Empty : m_LocalisationService.Get(help);
        }

        protected virtual async Task OnShowAnimationAsync()
        {
            await Awaitable.NextFrameAsync();
        }

        protected virtual async Task OnHideAnimationAsync()
        {
            await Awaitable.NextFrameAsync();
        }
    }
}
