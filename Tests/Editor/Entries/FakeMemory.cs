using System;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;

namespace RPGFramework.Menu.Tests.Entries
{
    /// <summary>Memory whose Bools read as set for their bank, false until set, remembering where one was last read.</summary>
    internal sealed class FakeMemory : IMemoryService
    {
        private const int BANKS = 4;

        private readonly bool[] m_Bools;

        internal int        Reads       { get; private set; }
        internal MemoryBank LastBank    { get; private set; }
        internal ushort     LastAddress { get; private set; }

        internal FakeMemory()
        {
            m_Bools     = new bool[BANKS];
            Reads       = 0;
            LastBank    = MemoryBank.Persistent;
            LastAddress = 0;
        }

        /// <summary>Sets what every Bool reads, in every bank.</summary>
        internal void SetBools(bool value)
        {
            for (int i = 0; i < m_Bools.Length; i++)
            {
                m_Bools[i] = value;
            }
        }

        /// <summary>Sets what every Bool of <paramref name="bank" /> reads.</summary>
        internal void SetBool(MemoryBank bank, bool value)
        {
            m_Bools[(int)bank] = value;
        }

        bool IMemoryService.ReadBool(MemoryBank bank, ushort address)
        {
            Reads++;
            LastBank    = bank;
            LastAddress = address;

            return m_Bools[(int)bank];
        }

        byte IMemoryService.ReadByte(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteByte(MemoryBank bank, ushort address, byte value) => throw new NotSupportedException();
        void IMemoryService.WriteBool(MemoryBank bank, ushort address, bool value) => throw new NotSupportedException();
        ushort IMemoryService.ReadUshort(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteUshort(MemoryBank bank, ushort address, ushort value) => throw new NotSupportedException();
        int IMemoryService.ReadInt(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteInt(MemoryBank bank, ushort address, int value) => throw new NotSupportedException();
        uint IMemoryService.ReadUint(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteUint(MemoryBank bank, ushort address, uint value) => throw new NotSupportedException();
        float IMemoryService.ReadFloat(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteFloat(MemoryBank bank, ushort address, float value) => throw new NotSupportedException();
        ulong IMemoryService.ReadUlong(MemoryBank bank, ushort address) => throw new NotSupportedException();
        void IMemoryService.WriteUlong(MemoryBank bank, ushort address, ulong value) => throw new NotSupportedException();
    }
}
