using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Localisation;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's clock and location as the sample lays them out, the location's text a dynamic text.</summary>
    internal sealed class WhenALayoutHoldsWhatItsUINeeds
    {
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            VisualElement layout   = new VisualElement();
            Label         location = new Label { name = "LocationLabel" };

            location.SetBinding("text", new DynamicLocalisedText { Sheet = "Locations" });

            layout.Add(new Label { name = "PlayTimeLabel" });
            layout.Add(location);

            m_Problems = new List<string>();

            MenuLayoutContentsCheck.CheckElements("Party", typeof(PartyMenuUI), layout, m_Problems);
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems);
        }
    }
}
