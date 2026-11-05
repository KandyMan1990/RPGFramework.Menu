using System.Collections.Generic;
using NUnit.Framework;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>The provider as the Menu Sample's is: a layout for every declared menu, and a save slot.</summary>
    internal sealed class WhenEveryMenuHasALayout
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            m_Problems = m_Menus.CheckLayouts();
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems);
        }
    }
}
