using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The leader's slot holding the roster's own field as a number, their name and their portrait.</summary>
    internal sealed class WhenAPartyMemberShowsWhatTheMapHolds
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            PartyMember leader = new PartyMember { PartySlot = 0 };

            leader.Add(new PartyMemberNumber { Field = CoreVariables.ROSTER_JOINED });
            leader.Add(new PartyMemberName());
            leader.Add(new PartyMemberPortrait());

            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckMembers(leader);
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems, string.Join("; ", m_Problems));
        }
    }
}
