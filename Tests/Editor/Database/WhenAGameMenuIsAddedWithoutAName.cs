using System.Collections.Generic;
using NUnit.Framework;
using RPGFramework.Hashing;

namespace RPGFramework.Menu.Tests.Database
{
    /// <summary>A game's Status menu added in code, with no name declared on it.</summary>
    internal sealed class WhenAGameMenuIsAddedWithoutAName
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

            m_Problems = MenusToCheck.CheckDatabase(database, m_Menus.Provider);
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
            StringAssert.Contains("IStatusMenu is in the menu database but declares no name", m_Problems[0]);
        }
    }
}
