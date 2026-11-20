using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Localisation;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's layout with its location but no clock.</summary>
    internal sealed class WhenALayoutLacksAnElement
    {
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            VisualElement layout   = new VisualElement();
            Label         location = new Label { name = "LocationLabel" };

            location.SetBinding("text", new DynamicLocalisedText { Sheet = "Locations" });
            layout.Add(location);

            m_Problems = new List<string>();

            MenuLayoutContentsCheck.CheckElements("Party", typeof(PartyMenuUI), layout, m_Problems);
        }

        [Test]
        public void ThenTheElementIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("Party: there is no Label named PlayTimeLabel, which PartyMenuUI finds by name", m_Problems[0]);
        }
    }
}
