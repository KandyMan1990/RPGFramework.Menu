using NUnit.Framework;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.Entries
{
    internal sealed class WhenAnEntryNamesACommand
    {
        private MenuChoice m_Choice;

        [SetUp]
        public void SetUp()
        {
            m_Choice = new MenuEntry { Command = BeginMenuCommands.NEW_GAME }.ToChoice();
        }

        [Test]
        public void ThenItRunsTheCommandItNames()
        {
            Assert.AreEqual(BeginMenuCommands.NewGame, m_Choice.Command);
        }

        [Test]
        public void ThenItOpensNoMenu()
        {
            Assert.AreEqual(0UL, m_Choice.Opens);
        }
    }
}
