using System;
using NUnit.Framework;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SharedTypes;
using UnityEngine;

namespace RPGFramework.Menu.Tests.Entries
{
    /// <summary>
    /// An entry greyed by Core's <c>SaveEnabled</c>, a Session Bool every new map holds as it is made.
    /// </summary>
    internal sealed class WhenAnEntryIsGreyedByAVariable
    {
        private VariableMapAsset m_Map;
        private FakeMemory       m_Memory;
        private string[]         m_When;

        [SetUp]
        public void SetUp()
        {
            m_Map    = ScriptableObject.CreateInstance<VariableMapAsset>();
            m_Memory = new FakeMemory();
            m_When   = new[] { CoreVariables.SAVE_ENABLED };
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(m_Map);
        }

        [Test]
        public void ThenItIsOfferedWhileTheVariableIsTrue()
        {
            m_Memory.SetBools(true);

            Assert.IsTrue(MenuEntryConditions.Holds(m_When, m_Map, m_Memory));
        }

        [Test]
        public void ThenItIsGreyedWhileTheVariableIsFalse()
        {
            m_Memory.SetBools(false);

            Assert.IsFalse(MenuEntryConditions.Holds(m_When, m_Map, m_Memory));
        }

        [Test]
        public void ThenTheVariablesBankIsRead()
        {
            MenuEntryConditions.Holds(m_When, m_Map, m_Memory);

            Assert.AreEqual(MemoryBank.Session, m_Memory.LastBank);
        }

        [Test]
        public void ThenTheVariableIsReadWhereTheMapPutsIt()
        {
            IVariableMap map = m_Map;

            map.TryGetVariable(CoreVariables.SAVE_ENABLED, out VariableDefinition variable);
            MenuEntryConditions.Holds(m_When, m_Map, m_Memory);

            Assert.AreEqual(variable.Offset, m_Memory.LastAddress);
        }

        [Test]
        public void ThenAnEntryNamingNoVariableIsOffered()
        {
            Assert.IsTrue(MenuEntryConditions.Holds(Array.Empty<string>(), m_Map, m_Memory));
        }

        [Test]
        public void ThenNoVariableIsReadForAnEntryNamingNone()
        {
            MenuEntryConditions.Holds(Array.Empty<string>(), m_Map, m_Memory);

            Assert.AreEqual(0, m_Memory.Reads);
        }

        [Test]
        public void ThenAVariableTheMapLacksIsRefusedInTheEditor()
        {
            Assert.Throws<InvalidOperationException>(() => MenuEntryConditions.Holds(new[] { "NoSuchVariable" }, m_Map, m_Memory));
        }
    }
}
