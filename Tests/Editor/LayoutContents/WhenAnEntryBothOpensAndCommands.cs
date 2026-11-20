using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>A title entry opening Load and naming New Game, so the command never runs.</summary>
    internal sealed class WhenAnEntryBothOpensAndCommands
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.BEGIN, new MenuEntry
            {
                name    = "LoadGameBtn",
                Opens   = MenuIds.LOAD,
                Command = BeginMenuCommands.NEW_GAME
            });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenBothAreReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("both opens a menu and names a command", m_Problems[0]);
        }
    }
}
