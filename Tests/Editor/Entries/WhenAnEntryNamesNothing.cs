using NUnit.Framework;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.Entries
{
    internal sealed class WhenAnEntryNamesNothing
    {
        private MenuChoice m_Choice;

        [SetUp]
        public void SetUp()
        {
            m_Choice = new MenuEntry().ToChoice();
        }

        [Test]
        public void ThenItOpensNoMenu()
        {
            Assert.AreEqual(0UL, m_Choice.Opens);
        }

        [Test]
        public void ThenNoVariableGreysIt()
        {
            Assert.IsNull(m_Choice.When);
        }
    }
}
