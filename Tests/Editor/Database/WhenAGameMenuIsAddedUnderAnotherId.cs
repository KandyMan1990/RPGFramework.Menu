using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Hashing;

namespace RPGFramework.Menu.Tests.Database
{
    /// <summary>A game's Status menu declared, and added under another menu's id.</summary>
    internal sealed class WhenAGameMenuIsAddedUnderAnotherId
    {
        private MenusToCheck m_Menus;
        private List<string> m_Problems;

        [SetUp]
        public void SetUp()
        {
            m_Menus = new MenusToCheck();

            MenuDatabase         database = new MenuDatabase();
            IMenuDatabaseBuilder builder  = database;
            builder.Add<IStatusMenu>(Fnv1a64.Hash("Equip"));

            m_Problems = MenusToCheck.CheckDatabase(database, m_Menus.Provider, new DeclaredMenu(MenusToCheck.STATUS, typeof(IStatusMenu)));
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
            StringAssert.Contains("IStatusMenu is declared as 'Status' but added to the menu database under another id", m_Problems[0]);
        }
    }
}
