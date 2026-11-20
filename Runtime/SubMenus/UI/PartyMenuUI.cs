using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class PartyMenuUI : MenuUI<IPartyMenuUI>, IPartyMenuUI
    {
        private const string PLAY_TIME_LABEL = "PlayTimeLabel";
        private const string LOCATION_LABEL  = "LocationLabel";
        private const string TEXT            = "text";

        [LayoutElement(PLAY_TIME_LABEL)]
        private Label m_PlayTimeLabel;

        [LayoutElement(LOCATION_LABEL, TEXT)]
        private DynamicLocalisedText m_LocationName;

        public PartyMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

        protected override void HookupUI()
        {
            m_PlayTimeLabel = m_UIInstance.Q<Label>(PLAY_TIME_LABEL);
            m_LocationName  = (DynamicLocalisedText)m_UIInstance.Q(LOCATION_LABEL).GetBinding(TEXT);
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
