using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public abstract class MenuUI<T> : IMenuUI where T : IMenu
    {
        private readonly IMenuUIProvider m_UIProvider;

        protected VisualElement m_RootUI;

        protected MenuUI(IMenuUIProvider uiProvider)
        {
            m_UIProvider = uiProvider;
        }

        protected abstract Task OnEnterAsync(VisualElement rootContainer);
        protected abstract Task OnSuspendAsync();
        protected abstract Task OnResumeAsync();
        protected abstract Task OnExitAsync();
        protected abstract void RegisterCallbacks();
        protected abstract void UnregisterCallbacks();

        async Task IMenuUI.OnEnterAsync(VisualElement rootContainer)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<T>();
            uiAsset.CloneTree(rootContainer);

            await OnEnterAsync(rootContainer);

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