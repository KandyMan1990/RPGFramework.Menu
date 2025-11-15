using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class BeginMenuUI : IBeginMenuUI
    {
        private readonly IMenuUIProvider m_UIProvider;

        public BeginMenuUI(IMenuUIProvider uiProvider)
        {
            m_UIProvider = uiProvider;
        }

        Task IMenuUI.OnEnterAsync(VisualElement rootContainer)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<IBeginMenu>();
            uiAsset.CloneTree(rootContainer);

            return Task.CompletedTask;
        }

        Task IMenuUI.OnSuspendAsync()
        {
            return Task.CompletedTask;
        }

        Task IMenuUI.OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        Task IMenuUI.OnExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}