using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>The party menu's UI with no class of a game's own beside it.</summary>
    internal sealed class WhenOnlyTheFrameworksUIIsThere
    {
        [Test]
        public void ThenItIsTheOneChecked()
        {
            Assert.AreEqual(typeof(PartyMenuUI), MenuUITypes.Choose(new[] { typeof(PartyMenuUI) }));
        }
    }
}
