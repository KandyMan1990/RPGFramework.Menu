using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.Editor;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>A provider whose every layout, and its save slot, is empty, read whole as the scene check reads it.</summary>
    internal sealed class WhenAProvidersLayoutsNameNothing
    {
        private MenusToCheck m_Menus;
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus    = new MenusToCheck();
            m_Names    = new NamesToCheck();
            m_Problems = new List<string>();

            MenuLayoutContentsCheck.Check(m_Menus.Provider, m_Names.Names, m_Problems);
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
            m_Names.Destroy();
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems);
        }
    }
}
