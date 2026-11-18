using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// A string property picked from a list of names, <c>None</c> first for empty, as an entry's attributes are in the UI
    /// Builder. A name not in the list stays as it was, for the build's check to report.
    /// </summary>
    internal static class NameDropdown
    {
        private const string NONE = "None";

        internal static DropdownField Create(SerializedProperty property, string[] names, Func<string, string> label)
        {
            List<string> choices = new List<string>(names.Length + 1) { NONE };

            choices.AddRange(names);

            DropdownField field = new DropdownField(property.displayName, choices, 0, label, label) { tooltip = property.tooltip };

            field.SetValueWithoutNotify(string.IsNullOrEmpty(property.stringValue) ? NONE : property.stringValue);
            field.AddToClassList(BaseField<string>.alignedFieldUssClassName);
            field.RegisterValueChangedCallback(e =>
            {
                property.stringValue = e.newValue == NONE ? string.Empty : e.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            return field;
        }
    }
}
