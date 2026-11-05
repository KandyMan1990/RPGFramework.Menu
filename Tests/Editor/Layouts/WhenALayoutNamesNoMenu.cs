using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>The party menu's layout under a mistyped name, so the party menu has none.</summary>
    internal sealed class WhenALayoutNamesNoMenu
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();
            m_Menus.RemoveLayout(MenuIds.PARTY);
            m_Menus.AddLayout("Partty");

            m_Problems = m_Menus.CheckLayouts();
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
        }

        [Test]
        public void ThenTheMenuWithoutOneIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("no layout for Party (IPartyMenu)", m_Problems[0]);
        }

        [Test]
        public void ThenTheNameIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("a layout for 'Partty', which no menu declares", m_Problems[1]);
        }
    }
}
