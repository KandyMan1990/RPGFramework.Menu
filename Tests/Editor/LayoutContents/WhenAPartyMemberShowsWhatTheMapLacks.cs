using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// A fourth slot of a party of three, a number showing a field the roster lacks, and a name outside any member.
    /// </summary>
    internal sealed class WhenAPartyMemberShowsWhatTheMapLacks
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            PartyMember fourth = new PartyMember { PartySlot = 3 };
            PartyMember leader = new PartyMember { PartySlot = 0 };

            leader.Add(new PartyMemberNumber { Field = "Level" });

            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckMembers(fourth, leader, new PartyMemberName());
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheSlotBeyondThePartyIsReported()
        {
            Assert.AreEqual(3, m_Problems.Count);
            StringAssert.Contains("a PartyMember shows slot 3, which the map's Party does not hold", m_Problems[0]);
        }

        [Test]
        public void ThenTheFieldTheRosterLacksIsReported()
        {
            Assert.AreEqual(3, m_Problems.Count);
            StringAssert.Contains("a PartyMemberNumber shows Level, which is no field of the map's Roster", m_Problems[1]);
        }

        [Test]
        public void ThenTheNameOutsideAMemberIsReported()
        {
            Assert.AreEqual(3, m_Problems.Count);
            StringAssert.Contains("a PartyMemberName is not inside a PartyMember", m_Problems[2]);
        }
    }
}
