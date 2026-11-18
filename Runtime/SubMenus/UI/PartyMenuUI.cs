using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class PartyMenuUI : MenuUI<IPartyMenuUI>, IPartyMenuUI
    {
        private Label m_PlayTimeLabel;

        private DynamicLocalisedText m_LocationName;

        public PartyMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_PlayTimeLabel = m_UIInstance.Q<Label>("PlayTimeLabel");
            m_LocationName  = (DynamicLocalisedText)m_UIInstance.Q<Label>("LocationLabel").GetBinding("text");
        }

        void IPartyMenuUI.SetLocationName(ulong keyHash)
        {
            m_LocationName.Hash = keyHash;

            Localise();
        }

        void IPartyMenuUI.SetPlayTime(uint seconds, bool colonLit)
        {
            m_PlayTimeLabel.text = ClockText.PlayTime(seconds, colonLit);
        }

        void IPartyMenuUI.SetCountdown(uint seconds, bool colonLit)
        {
            m_PlayTimeLabel.text = ClockText.Countdown(seconds, colonLit);
        }
    }
}
