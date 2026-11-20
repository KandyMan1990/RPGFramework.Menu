using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;
using RPGFramework.Menu.UI;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>Config's layout with its language row a plain label, which the cursor would never reach.</summary>
    internal sealed class WhenAConfigRowIsNotAnEntry
    {
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            VisualElement layout     = new VisualElement();
            string[]      sliderRows = { "MusicVolume", "SfxVolume", "BattleMessageSpeed", "FieldMessageSpeed" };

            layout.Add(new Label { name = "LanguageBtn" });

            for (int i = 0; i < sliderRows.Length; i++)
            {
                layout.Add(new MenuEntry { name = sliderRows[i] + "Btn" });
                layout.Add(new Slider { name = sliderRows[i] + "Slider" });
            }

            m_Problems = new List<string>();

            MenuLayoutContentsCheck.CheckElements("Config", typeof(ConfigMenuUI), layout, m_Problems);
        }

        [Test]
        public void ThenTheRowIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("Config: LanguageBtn is a Label, where ConfigMenuUI finds a MenuEntry", m_Problems[0]);
        }
    }
}
