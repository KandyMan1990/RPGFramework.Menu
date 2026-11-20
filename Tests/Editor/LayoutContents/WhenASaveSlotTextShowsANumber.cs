using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>A save slot's text showing Core's <c>PlayTime</c>, a UInt, where it reads a key's hash.</summary>
    internal sealed class WhenASaveSlotTextShowsANumber
    {
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Names    = new NamesToCheck();
            m_Problems = m_Names.CheckSaveSlot(new SaveSlotText { Variable = CoreVariables.PLAY_TIME });
        }

        [TearDown]
        public void TearDown()
        {
            m_Names.Destroy();
        }

        [Test]
        public void ThenItsWidthIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("a SaveSlotText shows PlayTime, a UInt, but reads a localisation key's hash, a ULong", m_Problems[0]);
        }
    }
}
