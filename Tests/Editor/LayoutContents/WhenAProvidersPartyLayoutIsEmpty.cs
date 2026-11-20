using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>A provider whose every layout is empty, read whole as the scene check reads it, the party menu's UI known.</summary>
    internal sealed class WhenAProvidersPartyLayoutIsEmpty
    {
        private MenusToCheck m_Menus;
        private NamesToCheck m_Names;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus    = new MenusToCheck();
            m_Names    = new NamesToCheck(MenuIds.PARTY, typeof(PartyMenuUI));
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
        public void ThenTheClockIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("(Party): there is no Label named PlayTimeLabel", m_Problems[0]);
        }

        [Test]
        public void ThenTheLocationIsReported()
        {
            Assert.AreEqual(2, m_Problems.Count);
            StringAssert.Contains("(Party): there is no element named LocationLabel", m_Problems[1]);
        }
    }
}
