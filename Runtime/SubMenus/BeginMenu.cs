using RPGFramework.Core;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => false;

        private readonly IMenuModule  m_MenuModule;
        private readonly IBeginMenuUI m_BeginMenuUI;

        public BeginMenu(IMenuModule  menuModule,
                         IBeginMenuUI beginMenuUI) : base(beginMenuUI)
        {
            m_MenuModule  = menuModule;
            m_BeginMenuUI = beginMenuUI;
        }
        protected override void RegisterCallbacks()
        {
            m_BeginMenuUI.OnPlayAudio += PlaySfx;
            m_BeginMenuUI.OnNewGame   += OnNewGame;
            m_BeginMenuUI.OnQuit      += OnQuit;
        }

        protected override void UnregisterCallbacks()
        {
            m_BeginMenuUI.OnQuit      -= OnQuit;
            m_BeginMenuUI.OnNewGame   -= OnNewGame;
            m_BeginMenuUI.OnPlayAudio -= PlaySfx;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }

        private void OnNewGame()
        {
            //TODO: tell core to init default save map
            m_MenuModule.ReturnToPreviousModuleAsync().FireAndForget();
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}