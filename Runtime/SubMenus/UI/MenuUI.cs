using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public abstract class MenuUI<TMenuUI> : IMenuUI where TMenuUI : IMenuUI
    {
        event Action<int> IMenuUI.OnPlayAudio
        {
            add => m_OnPlayAudio += value;
            remove => m_OnPlayAudio -= value;
        }
        event Action IMenuUI.OnBackButtonPressed
        {
            add => m_OnBackButtonPressed += value;
            remove => m_OnBackButtonPressed -= value;
        }

        VisualElement IMenuUI.GetDefaultFocusedElement() => GetDefaultFocusedElement();

        private event Action<int> m_OnPlayAudio;
        private event Action      m_OnBackButtonPressed;

        protected abstract VisualElement GetDefaultFocusedElement();

        protected readonly ILocalisationArgs       m_LocalisationArgs;
        protected readonly IMenuUIProvider         m_UIProvider;
        protected readonly IGenericAudioIdProvider m_AudioIdProvider;
        protected readonly ILocalisationService    m_LocalisationService;

        protected VisualElement m_UIInstance;

        protected MenuUI(ILocalisationArgs       localisationArgs,
                         IMenuUIProvider         uiProvider,
                         IGenericAudioIdProvider audioIdProvider,
                         ILocalisationService    localisationService)
        {
            m_LocalisationArgs    = localisationArgs;
            m_UIProvider          = uiProvider;
            m_AudioIdProvider     = audioIdProvider;
            m_LocalisationService = localisationService;
        }

        async Task IMenuUI.OnEnterAsync(VisualElement parent, Dictionary<string, object> args)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<TMenuUI>();
            uiAsset.CloneTree(parent);

            m_UIInstance = parent[parent.childCount - 1];

            ShowUI(false);

            await m_LocalisationService.LoadNewLocalisationDataAsync(m_LocalisationArgs.DataSheetsToLoad);

            await OnEnterAsync(args);

            HookupUI();

            LocaliseUI();

            RegisterCallbacks();

            ShowUI(true);

            GetDefaultFocusedElement()?.Focus();

            OnEnterComplete();
        }

        Task IMenuUI.OnSuspendAsync(bool hideUi)
        {
            UnregisterCallbacks();

            ShowUI(!hideUi);

            return OnSuspendAsync(hideUi);
        }

        async Task IMenuUI.OnResumeAsync()
        {
            await m_LocalisationService.LoadNewLocalisationDataAsync(m_LocalisationArgs.DataSheetsToLoad);

            LocaliseUI();

            RegisterCallbacks();

            ShowUI(true);

            await OnResumeAsync();
        }

        async Task IMenuUI.OnExitAsync()
        {
            UnregisterCallbacks();

            await OnExitAsync();

            m_UIInstance.RemoveFromHierarchy();
            m_UIInstance = null;
        }

        protected virtual Task OnEnterAsync(Dictionary<string, object> args)
        {
            return Task.CompletedTask;
        }

        protected virtual void OnEnterComplete()
        {

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

        protected void RaiseOnPlayAudio(int audio)
        {
            m_OnPlayAudio?.Invoke(audio);
        }

        protected void RaiseOnBackButtonPressed()
        {
            m_OnBackButtonPressed?.Invoke();
        }

        protected void OnBtnFocus(FocusInEvent evt)
        {
            RaiseOnPlayAudio(m_AudioIdProvider.ButtonNavigate);
        }
    }
}