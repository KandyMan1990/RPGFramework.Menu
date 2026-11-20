using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// A save slot showing a variable the map does not declare, and Core's <c>SaveEnabled</c>, which is in the Session
    /// bank and so in no save.
    /// </summary>
    internal sealed class WhenASaveSlotNamesNoPersistentValue
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckSaveSlot(new SaveSlotNumber { Variable = "Gil" },
                                               new SaveSlotNumber { Variable = CoreVariables.SAVE_ENABLED });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenTheUndeclaredOneIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("a SaveSlotNumber shows Gil, which is no persistent value", m_Problems[0]);
        }

        [Test]
        public void ThenTheSessionOneIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("a SaveSlotNumber shows SaveEnabled, which is no persistent value", m_Problems[1]);
        }
    }
}
