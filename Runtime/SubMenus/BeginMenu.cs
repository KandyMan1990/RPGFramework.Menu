using RPGFramework.Core;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class BeginMenu : Menu<IBeginMenuUI>, IBeginMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

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
            m_BeginMenuUI.OnNewGamePressed   += OnNewGamePressed;
            m_BeginMenuUI.OnSettingsPressed  += OnSettingsPressed;
            m_BeginMenuUI.OnQuitPressed      += OnQuitPressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_BeginMenuUI.OnQuitPressed      -= OnQuitPressed;
            m_BeginMenuUI.OnSettingsPressed  -= OnSettingsPressed;
            m_BeginMenuUI.OnNewGamePressed   -= OnNewGamePressed;
            m_BeginMenuUI.OnPlayAudio -= PlaySfx;
        }

        private void PlaySfx(int id)
        {
            m_MenuModule.PlaySfx(id);
        }

        private void OnNewGamePressed()
        {
            //TODO: tell core to init default save map
            m_MenuModule.ReturnToPreviousModuleAsync().FireAndForget();
        }

        private void OnSettingsPressed()
        {
            IMenuModuleArgs args = new MenuModuleArgs<IConfigMenu>();

            m_MenuModule.PushMenu(args).FireAndForget();
        }

        private void OnQuitPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}