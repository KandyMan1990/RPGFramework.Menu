using System;
using System.Diagnostics;

namespace RPGFramework.Menu
{
    /// <summary>
    /// Authoring only. The name a menu is opened by, on the interface it is bound to —
    /// <c>[MenuName(MenuIds.PARTY)]</c> — so the editor knows every menu, the framework's and a game's: the Menu UI
    /// Provider's inspector offers each a layout, and one with none is reported before the game runs.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false)]
    public sealed class MenuNameAttribute : Attribute
    {
#if UNITY_EDITOR
        internal string Name { get; }
#endif

        public MenuNameAttribute(string name)
        {
#if UNITY_EDITOR
            Name = name;
#endif
        }
    }
}
