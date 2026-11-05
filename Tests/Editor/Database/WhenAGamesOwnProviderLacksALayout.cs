using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Hashing;

namespace RPGFramework.Menu.Tests.Database
{
    /// <summary>A UI provider of the game's own, with a layout for every menu but the Status menu it added.</summary>
    internal sealed class WhenAGamesOwnProviderLacksALayout
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            MenuDatabase         database = new MenuDatabase();
            IMenuDatabaseBuilder builder  = database;
            GameMenuUIProvider   provider = new GameMenuUIProvider();

            for (int i = 0; i < MenusToCheck.Framework.Length; i++)
            {
                provider.Add(Fnv1a64.Hash(MenusToCheck.Framework[i].Name), m_Menus.Layout);
            }

            builder.Add<IStatusMenu>(Fnv1a64.Hash(MenusToCheck.STATUS));

            m_Problems = MenusToCheck.CheckDatabase(database, provider, new DeclaredMenu(MenusToCheck.STATUS, typeof(IStatusMenu)));
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
        }

        [Test]
        public void ThenItIsReported()
        {
            Assert.AreEqual(1, m_Problems.Count);
            StringAssert.Contains("no layout for IStatusMenu ('Status')", m_Problems[0]);
        }
    }
}
