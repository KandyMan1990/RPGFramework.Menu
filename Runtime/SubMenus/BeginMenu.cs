using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : IBeginMenu
    {
        private readonly IMenuUIProvider m_UIProvider;

        public BeginMenu(IMenuUIProvider uiProvider)
        {
            m_UIProvider = uiProvider;
        }

        Task IMenu.OnEnterAsync(VisualElement rootContainer)
        {
            VisualTreeAsset uiAsset = m_UIProvider.GetMenuUI<IBeginMenu>();
            uiAsset.CloneTree(rootContainer);

            return Task.CompletedTask;
        }

        Task IMenu.OnSuspendAsync()
        {
            return Task.CompletedTask;
        }

        Task IMenu.OnResumeAsync()
        {
            return Task.CompletedTask;
        }

        Task IMenu.OnExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}