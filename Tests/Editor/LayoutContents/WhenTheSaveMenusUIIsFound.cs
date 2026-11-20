using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>Save and Load, two menus built on one slot list, read for the UI their layouts are shown by.</summary>
    internal sealed class WhenTheSaveMenusUIIsFound
    {
        [Test]
        public void ThenSavesIsTheSlotList()
        {
            Assert.AreEqual(typeof(ISaveSlotMenuUI), MenuUITypes.UIInterfaceOf(typeof(SaveMenu)));
        }

        [Test]
        public void ThenLoadsIsTheSlotList()
        {
            Assert.AreEqual(typeof(ISaveSlotMenuUI), MenuUITypes.UIInterfaceOf(typeof(LoadMenu)));
        }
    }
}
