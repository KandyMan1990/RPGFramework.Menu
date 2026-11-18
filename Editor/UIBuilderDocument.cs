using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// The layout open in the UI Builder, for a drawer to offer only what suits it. The UI Builder names it in nothing
    /// public, so this reads its internal <c>Builder.ActiveWindow.document.visualTreeAsset</c> — at edit time only — and
    /// gives none should a Unity version change it.
    /// </summary>
    internal static class UIBuilderDocument
    {
        private const BindingFlags MEMBERS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        internal static VisualTreeAsset Open()
        {
            Type   builder = Type.GetType("Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule");
            object window  = Read(builder, null, "ActiveWindow");
            object open    = window == null ? null : Read(window.GetType(), window, "document");

            return open == null ? null : Read(open.GetType(), open, "visualTreeAsset") as VisualTreeAsset;
        }

        private static object Read(Type type, object target, string property)
        {
            return type?.GetProperty(property, MEMBERS)?.GetValue(target);
        }
    }
}
