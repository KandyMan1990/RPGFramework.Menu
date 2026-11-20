using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// The party menu's UI beside a class of a game's own. A plain class of the tests' stands for the game's: no test
    /// class may implement a framework UI's interface, or <c>TypeCache</c> would find it in the project's real check.
    /// </summary>
    internal sealed class WhenAGamesUIReplacesTheFrameworks
    {
        [Test]
        public void ThenTheGamesIsTheOneChecked()
        {
            Assert.AreEqual(typeof(MenusToCheck), MenuUITypes.Choose(new[] { typeof(PartyMenuUI), typeof(MenusToCheck) }));
        }
    }
}
