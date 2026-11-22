using System;
using System.Collections.Generic;
using RPGFramework.Core.Databases;
using RPGFramework.Core.SaveData;
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

        private readonly ICharacterDatabase m_Characters;

        private PartyMember[] m_Members;
        private SavePreview   m_SavePreview;

        public PartyMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService, ICharacterDatabase characters)
            : base(uiProvider, localisationService)
        {
            m_Characters  = characters;
            m_Members     = Array.Empty<PartyMember>();
            m_SavePreview = null;
        }

        protected override void HookupUI()
        {
            m_PlayTimeLabel = m_UIInstance.Q<Label>(PLAY_TIME_LABEL);
            m_LocationName  = (DynamicLocalisedText)m_UIInstance.Q(LOCATION_LABEL).GetBinding(TEXT);

            List<PartyMember> members = new List<PartyMember>();

            m_UIInstance.Query<PartyMember>().ToList(members);

            m_Members = members.ToArray();
        }

        protected override void LocaliseUI()
        {
            if (m_SavePreview == null)
            {
                return;
            }

            for (int i = 0; i < m_Members.Length; i++)
            {
                m_Members[i].Show(m_SavePreview, m_Characters, m_LocalisationService);
            }
        }

        void IPartyMenuUI.ShowParty(SavePreview playthrough)
        {
            m_SavePreview = playthrough;

            Localise();
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
