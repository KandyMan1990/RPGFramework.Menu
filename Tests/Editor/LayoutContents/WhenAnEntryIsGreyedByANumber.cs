using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Save greyed by Core's <c>PlayTime</c>, a persistent UInt.</summary>
    internal sealed class WhenAnEntryIsGreyedByANumber
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "SaveBtn", Opens = MenuIds.SAVE, When = CoreVariables.PLAY_TIME });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheVariableIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("the entry 'SaveBtn' is greyed by PlayTime, which is not a single Bool outside Temp", m_Problems[0]);
        }
    }
}
