using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.Entries
{
    internal sealed class WhenAnEntryIsReadFromItsLayout
    {
        private MenuChoice m_Choice;

        [SetUp]
        public void SetUp()
        {
            m_Choice = new MenuEntry { Opens = MenuIds.SAVE, When = new[] { "SaveUnsealed" } }.ToChoice();
        }

        [Test]
        public void ThenItOpensTheMenuItNames()
        {
            Assert.AreEqual(MenuIds.Save, m_Choice.Opens);
        }

        [Test]
        public void ThenItIsGreyedByTheVariableItNames()
        {
            CollectionAssert.AreEqual(new[] { "SaveUnsealed" }, m_Choice.When);
        }
    }
}
