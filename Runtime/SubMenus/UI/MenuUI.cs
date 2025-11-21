using System;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public abstract class MenuUI<T> : IMenuUI where T : IMenu
    {
        private readonly IMenuUIProvider m_UIProvider;

        protected readonly IGenericAudioIdProvider m_AudioIdProvider;

        protected VisualElement m_RootUI;

        private event Action<int> OnPlayAudio;

        protected MenuUI(IMenuUIProvider uiProvider, IGenericAudioIdProvider audioIdProvider)
        {
            m_UIProvider      = uiProvider;
            m_AudioIdProvider = audioIdProvider;
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
            OnPlayAudio?.Invoke(audio);
        }

        event Action<int> IMenuUI.OnPlayAudio
        {
            add => OnPlayAudio += value;
            remove => OnPlayAudio -= value;
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