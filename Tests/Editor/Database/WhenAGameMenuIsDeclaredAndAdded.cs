using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Hashing;

namespace RPGFramework.Menu.Tests.Database
{
    /// <summary>A game's Status menu declared, and added under its name's hash.</summary>
    internal sealed class WhenAGameMenuIsDeclaredAndAdded
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            MenuDatabase         database = new MenuDatabase();
            IMenuDatabaseBuilder builder  = database;
            builder.Add<IStatusMenu>(Fnv1a64.Hash(MenusToCheck.STATUS));

            m_Problems = MenusToCheck.CheckDatabase(database, m_Menus.Provider, new DeclaredMenu(MenusToCheck.STATUS, typeof(IStatusMenu)));
        }

        [TearDown]
        public void TearDown()
        {
            m_Menus.Destroy();
        }

        [Test]
        public void ThenNothingIsReported()
        {
            Assert.IsEmpty(m_Problems);
        }
    }
}
