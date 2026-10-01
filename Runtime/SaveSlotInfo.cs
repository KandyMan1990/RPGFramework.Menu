using System;

namespace RPGFramework.Menu
{
    public readonly struct SaveSlotInfo
    {
        public readonly ulong    LocationName;
        public readonly uint     PlayTime;
        public readonly DateTime LastWritten;
        public readonly bool     FromNewerVersion;
        public readonly bool     Damaged;

        public SaveSlotInfo(ulong    locationName,
                            uint     playTime,
                            DateTime lastWritten,
                            bool     fromNewerVersion,
                            bool     damaged)
        {
            LocationName     = locationName;
            PlayTime         = playTime;
            LastWritten      = lastWritten;
            FromNewerVersion = fromNewerVersion;
            Damaged          = damaged;
        }
    }
}