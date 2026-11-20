using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Save opening a menu under a mistyped name.</summary>
    internal sealed class WhenAnEntryOpensAnUndeclaredMenu
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "SaveBtn", Opens = "Savee" });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheMenuItOpensIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("Party: the entry 'SaveBtn' opens 'Savee', which no menu declares", m_Problems[0]);
        }
    }
}
