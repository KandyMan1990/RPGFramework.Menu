using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's Save as the sample lays it out, and the title's New Game.</summary>
    internal sealed class WhenEntriesNameWhatTheProjectHolds
    {
        private NamesToCheck m_Names;
        private List<string> m_PartyProblems;
        private List<string> m_TitleProblems;

        [SetUp]
        public void SetUp()
        {
            m_Names = new NamesToCheck();

            m_PartyProblems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry
            {
                name  = "SaveBtn",
                Opens = MenuIds.SAVE,
                When  = CoreVariables.SAVE_ENABLED,
                Help  = NamesToCheck.HELP
            });

            m_TitleProblems = m_Names.CheckEntries(MenuIds.BEGIN, new MenuEntry { name = "NewGameBtn", Command = BeginMenuCommands.NEW_GAME });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenThePartysEntryPasses()
        {
            Assert.IsEmpty(m_PartyProblems);
        }

        [Test]
        public void ThenTheTitlesCommandPasses()
        {
            Assert.IsEmpty(m_TitleProblems);
        }
    }
}
