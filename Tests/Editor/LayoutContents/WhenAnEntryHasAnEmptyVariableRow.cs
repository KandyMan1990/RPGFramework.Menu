using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// The party menu's Save greyed by Core's <c>SaveEnabled</c> and an empty name, as a row of the UI Builder's list
    /// left at None writes: <c>when="SaveEnabled,"</c>.
    /// </summary>
    internal sealed class WhenAnEntryHasAnEmptyVariableRow
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckEntries(MenuIds.PARTY, new MenuEntry
            {
                name  = "SaveBtn",
                Opens = MenuIds.SAVE,
                When  = new[] { CoreVariables.SAVE_ENABLED, string.Empty }
            });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheEmptyRowIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("the entry 'SaveBtn' is greyed by an empty name, a row of its When left at None", m_Problems[0]);
        }
    }
}
