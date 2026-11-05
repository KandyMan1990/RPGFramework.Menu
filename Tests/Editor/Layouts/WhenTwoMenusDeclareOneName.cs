using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>A game's menu declared under the name the party menu has.</summary>
    internal sealed class WhenTwoMenusDeclareOneName
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            m_Problems = m_Menus.CheckLayouts(new DeclaredMenu(MenuIds.PARTY, typeof(IStatusMenu)));
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
            StringAssert.Contains("'Party' is declared by both IPartyMenu and IStatusMenu", m_Problems[0]);
        }
    }
}
