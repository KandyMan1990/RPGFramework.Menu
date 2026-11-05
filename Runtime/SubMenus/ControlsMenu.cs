using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Settings;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    /// <summary>
    /// The player's own bindings: choosing a control's button waits for the control to bind it to, and Reset to defaults
    /// puts every binding back. Saved with the other settings as it closes.
    /// </summary>
    internal sealed class ControlsMenu : Menu<IControlsMenuUI>, IControlsMenu
    {
        private const string NO_CONTROL = "-";

        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly IInputRebindingService m_RebindingService;
        private readonly ISettingsService       m_SettingsService;

        private bool m_Waiting;

        public ControlsMenu(IControlsMenuUI        controlsMenuUI,
                            IInputRouter           inputRouter,
                            IMenuModule            menuModule,
                            IAudioIntentPlayer     audioIntentPlayer,
                            IInputRebindingService rebindingService,
                            ISettingsService       settingsService) : base(controlsMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_RebindingService = rebindingService;
            m_SettingsService  = settingsService;
            m_Waiting          = false;

            m_MenuUI.OnControlChosen += OnControlChosen;
            m_MenuUI.OnResetPressed  += OnResetPressed;
        }

        protected override Task OnEnterComplete()
        {
            ShowControlNames();

            return base.OnEnterComplete();
        }

        protected override Task OnExitAsync()
        {
            m_SettingsService.Commit();

            return base.OnExitAsync();
        }

        protected override bool HandleAction(ulong action)
        {
            if (!m_Waiting && action == CoreActions.Cancel)
            {
                Play(AudioIntent.Cancel);
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private void OnControlChosen(int button)
        {
            if (m_Waiting)
            {
                return;
            }

            Play(AudioIntent.Confirm);

            WaitForControl(button).FireAndForget();
        }

        private async Task WaitForControl(int button)
        {
            m_Waiting = true;

            SetInputEnabled(false);
            m_MenuUI.ShowPrompt(true);

            RebindOutcome outcome = await m_RebindingService.RebindAsync(m_MenuUI.GetControl(button), m_MenuUI.GetScheme(button), m_MenuUI.Controls);

            m_MenuUI.ShowPrompt(false);
            ShowControlNames();
            Play(outcome == RebindOutcome.Bound ? AudioIntent.Confirm : outcome == RebindOutcome.Refused ? AudioIntent.Error : AudioIntent.Cancel);

            // The press that bound it, still held, is not also a press on the page.
            await Task.Yield();

            SetInputEnabled(true);
            m_Waiting = false;
        }

        private void OnResetPressed()
        {
            if (m_Waiting)
            {
                return;
            }

            m_RebindingService.ResetAll();
            ShowControlNames();
            Play(AudioIntent.Confirm);
        }

        private void ShowControlNames()
        {
            for (int i = 0; i < m_MenuUI.ButtonCount; i++)
            {
                string name = m_RebindingService.ControlName(m_MenuUI.GetControl(i), m_MenuUI.GetScheme(i));

                m_MenuUI.SetControlName(i, name.Length > 0 ? name : NO_CONTROL);
            }
        }

        private void Play(AudioIntent intent)
        {
            m_AudioIntentPlayer.PlayAsync(intent, AudioContext.Menu).FireAndForget();
        }
    }
}
