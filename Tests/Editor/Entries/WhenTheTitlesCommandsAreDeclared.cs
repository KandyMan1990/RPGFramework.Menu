using System;
using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus;

namespace RPGFramework.Menu.Tests.Entries
{
    internal sealed class WhenTheTitlesCommandsAreDeclared
    {
        private DeclaredMenuCommand[] m_Commands;

        [SetUp]
        public void SetUp()
        {
            m_Commands = DeclaredMenuCommands.FindAll();
        }

        [Test]
        public void ThenNewGameIsOfferedAsTheTitles()
        {
            Assert.IsTrue(Array.Exists(m_Commands, command => command.Name == BeginMenuCommands.NEW_GAME && command.Menu == MenuIds.BEGIN));
        }

        [Test]
        public void ThenQuitIsOfferedAsTheTitles()
        {
            Assert.IsTrue(Array.Exists(m_Commands, command => command.Name == BeginMenuCommands.QUIT && command.Menu == MenuIds.BEGIN));
        }
    }
}
