using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Load, its help a key its sheet has not recorded.</summary>
    internal sealed class WhenAnEntrysHelpIsNoSheetsKey
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "LoadBtn", Opens = MenuIds.LOAD, Help = "PartyMenu/LoadHelp" });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheKeyIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("the entry 'LoadBtn' has the help key PartyMenu/LoadHelp, which no sheet holds", m_Problems[0]);
        }
    }
}
