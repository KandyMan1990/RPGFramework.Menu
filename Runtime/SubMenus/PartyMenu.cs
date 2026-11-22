using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Memory;
using RPGFramework.Core.PlayerLoop;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Core.UI;
using UnityEngine;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    /// <summary>
    /// The party menu, its entries its layout's, Save offered only while saving is allowed, the party as the playthrough
    /// holds it, and its clock live while it shows: the play time, or the countdown in its place while it is on screen.
    /// </summary>
    internal sealed class PartyMenu : Menu<IPartyMenuUI>, IPartyMenu, IUpdatable
    {
        protected override bool m_HidePreviousUIOnSuspend => true;

        private readonly ISaveEnabledStore  m_SaveEnabledStore;
        private readonly ILocationNameStore m_LocationNameStore;
        private readonly IPlayTimeStore     m_PlayTimeStore;
        private readonly ICountdownStore    m_CountdownStore;
        private readonly ISaveDataService   m_SaveDataService;
        private readonly ClockBlink         m_Blink;

        public PartyMenu(IPartyMenuUI       partyMenuUI,
                         IInputRouter       inputRouter,
                         IMenuModule        menuModule,
                         ISfxPlayer         sfxPlayer,
                         ISystemSoundConfig systemSounds,
                         IVariableMap       variableMap,
                         IMemoryService     memoryService,
                         ISaveEnabledStore  saveEnabledStore,
                         ILocationNameStore locationNameStore,
                         IPlayTimeStore     playTimeStore,
                         ICountdownStore    countdownStore,
                         ISaveDataService   saveDataService) : base(partyMenuUI,
                                                                   inputRouter,
                                                                   menuModule,
                                                                   sfxPlayer,
                                                                   systemSounds,
                                                                   variableMap,
                                                                   memoryService)
        {
            m_SaveEnabledStore  = saveEnabledStore;
            m_LocationNameStore = locationNameStore;
            m_PlayTimeStore     = playTimeStore;
            m_CountdownStore    = countdownStore;
            m_SaveDataService   = saveDataService;
            m_Blink             = new ClockBlink();
        }

        protected override bool CanOpen(ulong menuId)
        {
            return menuId != MenuIds.Save || m_SaveEnabledStore.IsSaveEnabled;
        }

        protected override Task OnEnterComplete()
        {
            m_MenuUI.SetLocationName(m_LocationNameStore.LocationName);
            m_MenuUI.ShowParty(m_SaveDataService.PreviewPlaythrough());

            ShowClock(0f);
            UpdateManager.RegisterUpdatable(this);

            return base.OnEnterComplete();
        }

        protected override Task OnSuspendAsync(bool hideUI)
        {
            UpdateManager.UnregisterUpdatable(this);

            return base.OnSuspendAsync(hideUI);
        }

        // A menu opened over this one may have changed the party or a member's values.
        protected override Task OnResumeAsync()
        {
            m_MenuUI.ShowParty(m_SaveDataService.PreviewPlaythrough());
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
                PlaySound(m_SystemSounds.Cancel);
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }
    }
}
