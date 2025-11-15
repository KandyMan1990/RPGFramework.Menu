using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : IBeginMenu
    {
        private readonly IBeginMenuUI m_BeginMenuUI;

        public BeginMenu(IBeginMenuUI beginMenuUI)
        {
            m_BeginMenuUI = beginMenuUI;
        }

        Task IMenu.OnEnterAsync(VisualElement rootContainer)
        {
            return m_BeginMenuUI.OnEnterAsync(rootContainer);
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