using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
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

        VisualElement IMenuUI.GetDefaultFocusedElement() => GetDefaultFocusedElement();
        VisualElement IMenuUI.GetLastFocusedElement()    => m_LastFocusedElement;

        private event Action m_OnBackButtonPressed;
        private event Action m_OnCursorMoved;

        protected abstract VisualElement GetDefaultFocusedElement();

        protected readonly ILocalisationArgs    m_LocalisationArgs;
        protected readonly IMenuUIProvider      m_UIProvider;
        protected readonly ILocalisationService m_LocalisationService;

        protected VisualElement m_UIInstance;

        protected VisualElement m_LastFocusedElement;

        private SelectionScope m_Selection;

        protected MenuUI(ILocalisationArgs    localisationArgs,
                         IMenuUIProvider      uiProvider,
                         ILocalisationService localisationService)
        {
            m_LocalisationArgs    = localisationArgs;
            m_UIProvider          = uiProvider;
            m_LocalisationService = localisationService;
            m_Selection           = null;
        }

        async Task IMenuUI.OnEnterAsync(VisualElement parent, Dictionary<string, object> args)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<TMenuUI>();
            uiAsset.CloneTree(parent);

            m_UIInstance = parent[parent.childCount - 1];
            m_Selection  = new SelectionScope(m_UIInstance);

            m_Selection.Moved += OnSelectionMoved;

            ShowUI(false);

            await m_LocalisationService.LoadNewLocalisationDataAsync(m_LocalisationArgs.DataSheetsToLoad);

            await OnEnterAsync(args);

            HookupUI();

            LocaliseUI();

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

            m_LocalisationService.UnloadLocalisationData(m_LocalisationArgs.DataSheetsToLoad);

            await OnSuspendAsync(hideUI);
        }

        async Task IMenuUI.OnResumeAsync()
        {
            await m_LocalisationService.LoadNewLocalisationDataAsync(m_LocalisationArgs.DataSheetsToLoad);

            LocaliseUI();

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
        }

        void IMenuUI.SetInputEnabled(bool enabled)
        {
            SetInputEnabled(enabled);
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
            m_LocalisationService.UnloadLocalisationData(m_LocalisationArgs.DataSheetsToLoad);

            return Task.CompletedTask;
        }

        protected virtual void HookupUI()
        {
        }

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