using System;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;

namespace RPGFramework.Menu.Tests.Entries
{
    /// <summary>Memory whose every Bool reads <see cref="Bool" />, remembering where it was last read.</summary>
    internal sealed class FakeMemory : IMemoryService
    {
        internal bool       Bool        { get; set; }
        internal int        Reads       { get; private set; }
        internal MemoryBank LastBank    { get; private set; }
        internal ushort     LastAddress { get; private set; }

        internal FakeMemory()
        {
            Bool        = false;
            Reads       = 0;
            LastBank    = MemoryBank.Persistent;
            LastAddress = 0;
        }

        bool IMemoryService.ReadBool(MemoryBank bank, ushort address)
        {
            Reads++;
            LastBank    = bank;
            LastAddress = address;

            return Bool;
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
