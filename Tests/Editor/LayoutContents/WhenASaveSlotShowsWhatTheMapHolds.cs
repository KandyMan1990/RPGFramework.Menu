using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The sample's save slot: the location, the money and the play time, each a persistent value Core requires.</summary>
    internal sealed class WhenASaveSlotShowsWhatTheMapHolds
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckSaveSlot(new SaveSlotText { Variable = CoreVariables.LOCATION_NAME },
                                               new SaveSlotNumber { Variable = CoreVariables.MONEY },
                                               new SaveSlotPlayTime());
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems);
        }
    }
}
