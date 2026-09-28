using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class PartyMenu : Menu<IPartyMenuUI>, IPartyMenu
    {
        protected override bool m_HidePreviousUiOnSuspend => true;

        private readonly ISaveEnabledStore  m_SaveEnabledStore;
        private readonly ILocationNameStore m_LocationNameStore;
        private readonly IPlayTimeStore     m_PlayTimeStore;

        private bool m_SaveEnabled;

        public PartyMenu(IPartyMenuUI       partyMenuUI,
                         IInputRouter       inputRouter,
                         IMenuModule        menuModule,
                         IAudioIntentPlayer audioIntentPlayer,
                         ISaveEnabledStore  saveEnabledStore,
                         ILocationNameStore locationNameStore,
                         IPlayTimeStore     playTimeStore) : base(partyMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_SaveEnabledStore  = saveEnabledStore;
            m_LocationNameStore = locationNameStore;
            m_PlayTimeStore     = playTimeStore;
        }

        protected override Task OnEnterAsync(Dictionary<string, object> args)
        {
            m_SaveEnabled = m_SaveEnabledStore.GetSaveEnabled;

            return base.OnEnterAsync(args);
        }

        protected override Task OnEnterComplete()
        {
            m_MenuUI.SetSaveEnabled(m_SaveEnabled);
            m_MenuUI.SetLocationName(m_LocationNameStore.GetLocationName);
            m_MenuUI.SetPlayTime(m_PlayTimeStore.GetPlayTime);

            return base.OnEnterComplete();
        }

        protected override Task OnResumeAsync()
        {
            m_MenuUI.SetPlayTime(m_PlayTimeStore.GetPlayTime);

            return base.OnResumeAsync();
        }

        protected override void RegisterCallbacks()
        {
            m_MenuUI.OnConfigPressed += OnConfigPressed;
            m_MenuUI.OnSavePressed   += OnSavePressed;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnSavePressed   -= OnSavePressed;
            m_MenuUI.OnConfigPressed -= OnConfigPressed;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            if (slot == ControlSlot.Secondary)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
                m_MenuModule.PopMenu().FireAndForget();
            }

            return true;
        }

        private void OnConfigPressed()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_MenuModule.PushMenu(MenuType.Config).FireAndForget();
        }

        private void OnSavePressed()
        {
            if (!m_SaveEnabled)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_MenuModule.PushMenu(MenuType.Save).FireAndForget();
        }
    }
}