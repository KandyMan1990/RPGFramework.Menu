using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests.Entries
{
    /// <summary>
    /// An entry of a layout beside this file, <c>SeveralVariables.uxml</c>, its <c>when</c> written
    /// <c>"SaveEnabled, CountdownShown"</c>: the comma-separated form Menu's README describes, as Unity reads it.
    /// </summary>
    internal sealed class WhenAnEntryNamesSeveralVariablesInItsLayout
    {
        private const string LAYOUT = "Packages/com.rpgframework.menu/Tests/Editor/Entries/SeveralVariables.uxml";

        private string[] m_When;

        [SetUp]
        public void SetUp()
        {
            VisualTreeAsset layout = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(LAYOUT);

            m_When = layout.Instantiate().Q<MenuEntry>().ToChoice().When;
        }

        [Test]
        public void ThenEachNameIsRead()
        {
            Assert.AreEqual(2, m_When.Length);
        }

        [Test]
        public void ThenTheSpacesAreTrimmed()
        {
            CollectionAssert.AreEqual(new[] { CoreVariables.SAVE_ENABLED, CoreVariables.COUNTDOWN_SHOWN }, m_When);
        }
    }
}
