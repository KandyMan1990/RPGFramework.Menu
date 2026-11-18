using System.Diagnostics;
using UnityEngine;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Marks a <c>string</c> as a menu's name, which the UI Builder offers from the menus declared with
    /// <see cref="MenuNameAttribute" />, empty for none. Left out of a player build: only the editor reads it.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    internal sealed class DeclaredMenuNameAttribute : PropertyAttribute
    {
    }

    /// <summary>
    /// Marks a <c>string</c> as a command's name, which the UI Builder offers from the commands declared with
    /// <see cref="MenuCommandAttribute" />, empty for none. Left out of a player build: only the editor reads it.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    internal sealed class DeclaredMenuCommandAttribute : PropertyAttribute
    {
    }
}
