using RPGFramework.Hashing;
using Unity.Scripting.LifecycleManagement;

namespace RPGFramework.Menu.SubMenus
{
    /// <summary>
    /// The title's commands, each the hash of its name, as an entry in its layout names one with <c>command</c>.
    /// </summary>
    [NoAutoStaticsCleanup]
    internal sealed class BeginMenuCommands
    {
        /// <summary>The name of the command beginning a new game.</summary>
        internal const string NEW_GAME = "NewGame";

        /// <summary>The name of the command quitting the game.</summary>
        internal const string QUIT = "Quit";

        internal static readonly ulong NewGame = Fnv1a64.Hash(NEW_GAME);

        internal static readonly ulong Quit = Fnv1a64.Hash(QUIT);
#if ENABLE_RPG_FRAMEWORK_DEBUG

        /// <summary>The name of the command opening the debug room, its entry added in code.</summary>
        internal const string DEBUG_ROOM = "DebugRoom";

        internal static readonly ulong DebugRoom = Fnv1a64.Hash(DEBUG_ROOM);
#endif
    }
}
