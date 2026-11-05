using System.Collections.Generic;
using NUnit.Framework;

namespace RPGFramework.Menu.Tests.Layouts
{
    /// <summary>A game's Status menu declared, and its layout never given.</summary>
    internal sealed class WhenADeclaredMenuHasNoLayout
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            m_Problems = m_Menus.CheckLayouts(new DeclaredMenu(MenusToCheck.STATUS, typeof(IStatusMenu)));
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
            StringAssert.Contains("no layout for Status (IStatusMenu)", m_Problems[0]);
        }
    }
}
