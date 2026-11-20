using System;
using System.Diagnostics;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Authoring only. A field a menu UI fills from its layout by name — <c>[LayoutElement(PLAY_TIME_LABEL)]</c> on
    /// <c>m_PlayTimeLabel</c>, the element of the field's type — or, given a bound property, with that property's
    /// binding on the element so named, of the field's type. The Menu UI Provider's check reads each against the menu's
    /// layout, so one renamed, removed, of another type or bound otherwise is reported before the menu opens.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed class LayoutElementAttribute : Attribute
    {
#if UNITY_EDITOR
        internal string Name          { get; }
        internal string BoundProperty { get; }
#endif

        public LayoutElementAttribute(string name, string boundProperty = null)
        {
#if UNITY_EDITOR
            Name          = name;
            BoundProperty = boundProperty;
#endif
        }
    }
}
