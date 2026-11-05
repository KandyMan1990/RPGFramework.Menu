using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.PlayerLoop;
using RPGFramework.Core.Store;
using RPGFramework.Core.UI;
using UnityEngine;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    /// <summary>
    /// The party menu, its clock live while it shows: the play time, or the countdown in its place while it is on
    /// screen.
    /// </summary>
    internal sealed class PartyMenu : Menu<IPartyMenuUI>, IPartyMenu, IUpdatable
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ISaveEnabledStore  m_SaveEnabledStore;
        private readonly ILocationNameStore m_LocationNameStore;
        private readonly IPlayTimeStore     m_PlayTimeStore;
        private readonly ICountdownStore    m_CountdownStore;
        private readonly ClockBlink         m_Blink;

        private bool m_SaveEnabled;

        public PartyMenu(IPartyMenuUI       partyMenuUI,
                         IInputRouter       inputRouter,
                         IMenuModule        menuModule,
                         IAudioIntentPlayer audioIntentPlayer,
                         ISaveEnabledStore  saveEnabledStore,
                         ILocationNameStore locationNameStore,
                         IPlayTimeStore     playTimeStore,
                         ICountdownStore    countdownStore) : base(partyMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_SaveEnabledStore  = saveEnabledStore;
            m_LocationNameStore = locationNameStore;
            m_PlayTimeStore     = playTimeStore;
            m_CountdownStore    = countdownStore;
            m_Blink             = new ClockBlink();

            m_MenuUI.OnConfigPressed += OnConfigPressed;
            m_MenuUI.OnSavePressed   += OnSavePressed;
            m_MenuUI.OnLoadPressed   += OnLoadPressed;
        }

        protected override Task OnEnterAsync(Dictionary<string, object> args)
        {
            m_SaveEnabled = m_SaveEnabledStore.IsSaveEnabled;

            return base.OnEnterAsync(args);
        }

        protected override Task OnEnterComplete()
        {
            m_MenuUI.SetSaveEnabled(m_SaveEnabled);
            m_MenuUI.SetLocationName(m_LocationNameStore.LocationName);

            ShowClock(0f);
            UpdateManager.RegisterUpdatable(this);

            return base.OnEnterComplete();
        }

        protected override Task OnSuspendAsync(bool hideUI)
        {
            UpdateManager.UnregisterUpdatable(this);

            return base.OnSuspendAsync(hideUI);
        }

        protected override Task OnResumeAsync()
        {
            ShowClock(0f);
            UpdateManager.RegisterUpdatable(this);

            return base.OnResumeAsync();
        }

        protected override Task OnExitAsync()
        {
            UpdateManager.UnregisterUpdatable(this);

            return base.OnExitAsync();
        }

        void IUpdatable.Update()
        {
            ShowClock(Time.unscaledDeltaTime);
        }

        // A countdown at zero has stopped, so its colon stays lit.
        private void ShowClock(float deltaTime)
        {
            if (m_CountdownStore.IsShown)
            {
                uint left = m_CountdownStore.Seconds;

                m_MenuUI.SetCountdown(left, left == 0 || m_Blink.Tick(left, deltaTime));
                return;
            }

            uint played = m_PlayTimeStore.PlayTime;

            m_MenuUI.SetPlayTime(played, m_Blink.Tick(played, deltaTime));
        }

        protected override bool HandleAction(ulong action)
        {
            if (action == CoreActions.Cancel)
            {
                m_AudioIntentPlayer.PlayAsync(AudioIntent.Cancel, AudioContext.Menu).FireAndForget();
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private void OnConfigPressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

        private void OnSavePressed()
        {
            if (!m_SaveEnabled)
            {
                m_AudioIntentPlayer.PlayAsync(AudioIntent.Error, AudioContext.Menu).FireAndForget();
                return;
            }

            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_MenuModule.PushMenu(MenuType.Save).FireAndForget();
        }

        private void OnLoadPressed()
        {
            m_AudioIntentPlayer.PlayAsync(AudioIntent.Navigate, AudioContext.Menu).FireAndForget();
            m_MenuModule.PushMenu(MenuType.Load).FireAndForget();
        }
    }
}