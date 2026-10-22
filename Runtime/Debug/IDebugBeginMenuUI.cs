#if ENABLE_RPG_FRAMEWORK_DEBUG
using System;

namespace RPGFramework.Menu
{
    /// <summary>The title menu's UI with the debug room's button, which it adds above Quit.</summary>
    internal interface IDebugBeginMenuUI
    {
        /// <summary>Raised when the player chooses Debug Room.</summary>
        event Action OnDebugRoomPressed;
    }
}
#endif
