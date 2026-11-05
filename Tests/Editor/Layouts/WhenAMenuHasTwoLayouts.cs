using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>Two layouts under the party menu's name.</summary>
    internal sealed class WhenAMenuHasTwoLayouts
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();
            m_Menus.AddLayout(MenuIds.PARTY);

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
            StringAssert.Contains("two layouts for 'Party'", m_Problems[0]);
        }
    }
}
