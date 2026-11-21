using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Save greyed by a variable the map does not declare.</summary>
    internal sealed class WhenAnEntryIsGreyedByNothingTheMapHolds
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "SaveBtn", Opens = MenuIds.SAVE, When = new[] { "SaveUnsealed" } });
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
            StringAssert.Contains("is greyed by SaveUnsealed, which the variable map does not hold", m_Problems[0]);
        }
    }
}
