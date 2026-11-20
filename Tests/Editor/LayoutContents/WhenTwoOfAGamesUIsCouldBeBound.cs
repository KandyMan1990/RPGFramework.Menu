using NUnit.Framework;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SubMenus.UI;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// The party menu's UI beside two classes of a game's own, plain classes of the tests' standing for them, so which
    /// the game binds cannot be told.
    /// </summary>
    internal sealed class WhenTwoOfAGamesUIsCouldBeBound
    {
        [Test]
        public void ThenNoneIsChecked()
        {
            Assert.IsNull(MenuUITypes.Choose(new[] { typeof(PartyMenuUI), typeof(MenusToCheck), typeof(NamesToCheck) }));
        }
    }
}
