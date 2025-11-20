using System;
using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public abstract class MenuUI<T> : IMenuUI where T : IMenu
    {
        private readonly IMenuUIProvider m_UIProvider;

        protected VisualElement m_RootUI;

        private event Action<int> m_OnPlayAudio;

        protected MenuUI(IMenuUIProvider uiProvider)
        {
            m_UIProvider = uiProvider;
        }

        protected abstract Task          OnEnterAsync(VisualElement rootContainer);
        protected abstract Task          OnSuspendAsync();
        protected abstract Task          OnResumeAsync();
        protected abstract Task          OnExitAsync();
        protected abstract void          RegisterCallbacks();
        protected abstract void          UnregisterCallbacks();
        protected abstract VisualElement ElementToFocusOnEnter { get; }

        protected void RaiseOnPlayAudio(int audio)
        {
            m_OnPlayAudio?.Invoke(audio);
        }

        event Action<int> IMenuUI.OnPlayAudio
        {
            add => m_OnPlayAudio += value;
            remove => m_OnPlayAudio -= value;
        }

        async Task IMenuUI.OnEnterAsync(VisualElement rootContainer)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<T>();
            uiAsset.CloneTree(rootContainer);

            await OnEnterAsync(rootContainer);

            ElementToFocusOnEnter?.Focus();

            RegisterCallbacks();
        }

        Task IMenuUI.OnSuspendAsync()
        {
            UnregisterCallbacks();

            return OnSuspendAsync();
        }

        async Task IMenuUI.OnResumeAsync()
        {
            await OnResumeAsync();

            RegisterCallbacks();
        }

        Task IMenuUI.OnExitAsync()
        {
            UnregisterCallbacks();

            return OnExitAsync();
        }
    }
}