using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The title's New Game copied into the party menu's layout, where nothing carries it out.</summary>
    internal sealed class WhenAnEntryNamesAnotherMenusCommand
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry { name = "NewGameBtn", Command = BeginMenuCommands.NEW_GAME });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheCommandIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("names the command 'NewGame', which Party does not declare", m_Problems[0]);
        }
    }
}
