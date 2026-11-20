using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Save, its help key written without its sheet, which the menu could not load.</summary>
    internal sealed class WhenAnEntrysHelpNamesNoSheet
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "SaveBtn", Opens = MenuIds.SAVE, Help = "SaveHelp" });
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
            StringAssert.Contains("has the help key SaveHelp, which does not name its sheet", m_Problems[0]);
        }
    }
}
