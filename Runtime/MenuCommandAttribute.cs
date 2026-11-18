using System;
using System.Diagnostics;

namespace RPGFramework.Menu
{
    /// <summary>
    /// Authoring only. A command a menu carries out itself, on the interface it is bound to — the title's
    /// <c>[MenuCommand("NewGame")]</c> — one attribute a command, so the UI Builder offers it for an entry's
    /// <c>command</c>.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MenuCommandAttribute : Attribute
    {
#if UNITY_EDITOR
        internal string Name { get; }
#endif

        public MenuCommandAttribute(string name)
        {
#if UNITY_EDITOR
            Name = name;
#endif
        }
    }
}
