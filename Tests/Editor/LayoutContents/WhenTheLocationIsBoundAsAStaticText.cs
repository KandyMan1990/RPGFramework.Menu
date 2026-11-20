using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Localisation;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's location given a fixed key, where its UI sets the key's hash as the field changes.</summary>
    internal sealed class WhenTheLocationIsBoundAsAStaticText
    {
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            VisualElement layout   = new VisualElement();
            Label         location = new Label { name = "LocationLabel" };

            location.SetBinding("text", new StaticLocalisedText { Key = "Locations/Balamb" });

            layout.Add(new Label { name = "PlayTimeLabel" });
            layout.Add(location);

            m_Problems = new List<string>();

            MenuLayoutContentsCheck.CheckElements("Party", typeof(PartyMenuUI), layout, m_Problems);
        }

        [Test]
        public void ThenTheBindingIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("LocationLabel's text is bound by a StaticLocalisedText, where PartyMenuUI reads a DynamicLocalisedText", m_Problems[0]);
        }
    }
}
