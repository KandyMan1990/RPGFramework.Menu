using System.Collections.Generic;
using NUnit.Framework;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>Every menu's layout given, but not the save slot's.</summary>
    internal sealed class WhenThereIsNoSaveSlot
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();
            m_Menus.RemoveSaveSlot();

            m_Problems = m_Menus.CheckLayouts();
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
        }

        [Test]
        public void ThenItIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("no Save Slot layout", m_Problems[0]);
        }
    }
}
