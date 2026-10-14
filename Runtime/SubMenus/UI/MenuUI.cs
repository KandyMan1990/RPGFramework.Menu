using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
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

        VisualElement IMenuUI.GetDefaultFocusedElement() => GetDefaultFocusedElement();
        VisualElement IMenuUI.GetLastFocusedElement()    => m_LastFocusedElement;

        private event Action m_OnBackButtonPressed;

        protected abstract VisualElement GetDefaultFocusedElement();

        protected readonly ILocalisationArgs    m_LocalisationArgs;
        protected readonly IMenuUIProvider      m_UIProvider;
        protected readonly IAudioIntentPlayer   m_AudioIntentPlayer;
        protected readonly ILocalisationService m_LocalisationService;

        protected VisualElement m_UIInstance;

        protected VisualElement m_LastFocusedElement;

        private bool m_InputEnabled;

        protected MenuUI(ILocalisationArgs    localisationArgs,
                         IMenuUIProvider      uiProvider,
                         IAudioIntentPlayer   audioIntentPlayer,
                         ILocalisationService localisationService)
        {
            m_LocalisationArgs    = localisationArgs;
            m_UIProvider          = uiProvider;
            m_AudioIntentPlayer   = audioIntentPlayer;
            m_LocalisationService = localisationService;
            m_InputEnabled        = false;
        }

        async Task IMenuUI.OnEnterAsync(VisualElement parent, Dictionary<string, object> args)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<TMenuUI>();
            uiAsset.CloneTree(parent);

            m_UIInstance = parent[parent.childCount - 1];

            RegisterRootCallbacks();

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
        }

        void IMenuUI.SetInputEnabled(bool enabled)
        {
            SetInputEnabled(enabled);
        }

        private void SetInputEnabled(bool enabled)
        {
            if (enabled == m_InputEnabled)
            {
                return;
            }

            m_InputEnabled = enabled;

            if (enabled)
            {
                RegisterCallbacks();
            }
            else
            {
                UnregisterCallbacks();
            }
        }

        // UI Toolkit moves focus by itself, to the next element on a navigation key and to a button the pointer enters
        // or presses, so while the menu takes no input its events stop at its root, before they reach one. The cursor
        // sound follows focus however it moved; the menu's own focusing, as it enters or resumes, comes while its input
        // is off and plays nothing.
        private void RegisterRootCallbacks()
        {
            m_UIInstance.RegisterCallback<NavigationMoveEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<NavigationSubmitEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<NavigationCancelEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<PointerEnterEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<PointerDownEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<ClickEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<WheelEvent>(StopIfInputDisabled, TrickleDown.TrickleDown);
            m_UIInstance.RegisterCallback<FocusInEvent>(OnFocusIn, TrickleDown.TrickleDown);
        }

        private void StopIfInputDisabled(EventBase evt)
        {
            if (m_InputEnabled)
            {
                return;
            }

            evt.StopImmediatePropagation();
            m_UIInstance.focusController.IgnoreEvent(evt);
        }

        private void OnFocusIn(FocusInEvent evt)
        {
            if (m_InputEnabled)
            {
                OnBtnNavigate();
            }
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

        protected virtual void RegisterCallbacks()
        {
        }

        protected virtual void UnregisterCallbacks()
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

        protected void OnBtnNavigate()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
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