using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SharedTypes;
using UnityEngine;

namespace RPGFramework.Menu.Tests.Entries
{
    /// <summary>
    /// An entry greyed by Core's <c>SaveEnabled</c>, a Session Bool, and <c>CountdownShown</c>, a Persistent one, both
    /// held by every new map.
    /// </summary>
    internal sealed class WhenAnEntryIsGreyedBySeveralVariables
    {
        private VariableMapAsset m_Map;
        private FakeMemory       m_Memory;
        private string[]         m_When;

        [SetUp]
        public void SetUp()
        {
            m_Map    = ScriptableObject.CreateInstance<VariableMapAsset>();
            m_Memory = new FakeMemory();
            m_When   = new[] { CoreVariables.SAVE_ENABLED, CoreVariables.COUNTDOWN_SHOWN };

            m_Memory.SetBools(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_Map);
        }

        [Test]
        public void ThenItIsOfferedWhileEveryOneIsTrue()
        {
            Assert.IsTrue(MenuEntryConditions.Holds(m_When, m_Map, m_Memory));
        }

        [Test]
        public void ThenItIsGreyedWhileTheFirstIsFalse()
        {
            m_Memory.SetBool(MemoryBank.Session, false);

            Assert.IsFalse(MenuEntryConditions.Holds(m_When, m_Map, m_Memory));
        }

        [Test]
        public void ThenItIsGreyedWhileTheLastIsFalse()
        {
            m_Memory.SetBool(MemoryBank.Persistent, false);

            Assert.IsFalse(MenuEntryConditions.Holds(m_When, m_Map, m_Memory));
        }

        [Test]
        public void ThenEachIsReadWhileTheyHold()
        {
            MenuEntryConditions.Holds(m_When, m_Map, m_Memory);

            Assert.AreEqual(2, m_Memory.Reads);
        }
    }
}
