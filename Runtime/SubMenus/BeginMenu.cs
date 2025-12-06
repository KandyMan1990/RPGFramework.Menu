using System.Threading.Tasks;
using RPGFramework.Menu.SharedTypes;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : IBeginMenu
    {
        private readonly IMenuModule  m_MenuModule;
        private readonly IBeginMenuUI m_BeginMenuUI;

        public BeginMenu(IMenuModule menuModule, IBeginMenuUI beginMenuUI)
        {
            m_MenuModule  = menuModule;
            m_BeginMenuUI = beginMenuUI;
        }

        Task IMenu.OnEnterAsync(VisualElement rootContainer)
        {
            m_BeginMenuUI.OnPlayAudio += PlaySfx;

            return m_BeginMenuUI.OnEnterAsync(rootContainer);
        }

        Task IMenu.OnSuspendAsync()
        {
            m_BeginMenuUI.OnPlayAudio -= PlaySfx;

            return Task.CompletedTask;
        }

        Task IMenu.OnResumeAsync()
        {
            m_BeginMenuUI.OnPlayAudio += PlaySfx;

            return Task.CompletedTask;
        }

        Task IMenu.OnExitAsync()
        {
            m_BeginMenuUI.OnPlayAudio -= PlaySfx;

            return Task.CompletedTask;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }
    }
}