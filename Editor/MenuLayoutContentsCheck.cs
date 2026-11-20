using System.Collections.Generic;
using RPGFramework.Core.Memory;
using RPGFramework.Menu.UI;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// What a Menu UI Provider's layouts name that the project does not hold: an entry's menu, command, variable and help
    /// key, and the persistent values its save slot shows. The UI Builder offers each from a list, but nothing notices one
    /// gone since — a menu renamed, a variable or key removed, a layout given to another menu, the file edited by hand —
    /// and in play each is a menu that throws as it opens, or a choice that does nothing.
    /// </summary>
    internal static class MenuLayoutContentsCheck
    {
        internal static void Check(MenuUIProvider provider, LayoutNames names, List<string> problems)
        {
            SerializedObject   serialized = new SerializedObject(provider);
            SerializedProperty layouts    = serialized.FindProperty("m_Layouts");

            for (int i = 0; i < layouts.arraySize; i++)
            {
                string          menu   = MenuLayoutsCheck.Name(layouts, i);
                VisualTreeAsset layout = MenuLayoutsCheck.Layout(layouts, i) as VisualTreeAsset;

                // A layout no menu declares is the structural check's to report; its commands would all be wrong here.
                if (layout != null && names.IsMenu(menu))
                {
                    CheckEntries($"{layout.name} ({menu})", menu, layout.Instantiate(), names, problems);
                }
            }

            if (serialized.FindProperty("m_SaveSlot").objectReferenceValue is VisualTreeAsset slot)
            {
                CheckSaveSlot($"{slot.name} (the save slot)", slot.Instantiate(), names, problems);
            }
        }

        /// <summary>The problems of the entries in <paramref name="layout" />, the layout of <paramref name="menu" />.</summary>
        internal static void CheckEntries(string where, string menu, VisualElement layout, LayoutNames names, List<string> problems)
        {
            List<MenuEntry> entries = layout.Query<MenuEntry>().ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                MenuEntry entry    = entries[i];
                string    which    = $"{where}: the entry {Describe(entry, i)}";
                bool      opens    = !string.IsNullOrEmpty(entry.Opens);
                bool      commands = !string.IsNullOrEmpty(entry.Command);

                if (opens && !names.IsMenu(entry.Opens))
                {
                    problems.Add($"{which} opens '{entry.Opens}', which no menu declares");
                }

                if (commands && !names.Declares(menu, entry.Command))
                {
                    problems.Add($"{which} names the command '{entry.Command}', which {menu} does not declare with [MenuCommand]");
                }

                if (opens && commands)
                {
                    problems.Add($"{which} both opens a menu and names a command: choosing it opens the menu, and the command never runs");
                }

                if (!string.IsNullOrEmpty(entry.When) && names.HasVariableMap)
                {
                    string problem = names.ProblemWithWhen(entry.When);

                    if (problem != null)
                    {
                        problems.Add($"{which} {problem}");
                    }
                }

                if (!string.IsNullOrEmpty(entry.Help))
                {
                    CheckHelp(which, entry.Help, names, problems);
                }
            }
        }

        /// <summary>The problems of the values a save slot's layout shows.</summary>
        internal static void CheckSaveSlot(string where, VisualElement slot, LayoutNames names, List<string> problems)
        {
            if (!names.HasVariableMap)
            {
                return;
            }

            List<VisualElement> elements = slot.Query<VisualElement>().ToList();

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] is SaveSlotText text)
                {
                    CheckValue(where, nameof(SaveSlotText), text.Variable, true, names, problems);
                }
                else if (elements[i] is SaveSlotNumber number)
                {
                    CheckValue(where, nameof(SaveSlotNumber), number.Variable, false, names, problems);
                }
                else if (elements[i] is SaveSlotPlayTime playTime)
                {
                    CheckValue(where, nameof(SaveSlotPlayTime), playTime.Variable, false, names, problems);
                }
            }
        }

        // As a localised text's key is checked: the sheet a missing one names is not shipped, and loading it would throw.
        private static void CheckHelp(string which, string help, LayoutNames names, List<string> problems)
        {
            if (help.IndexOf('/') <= 0)
            {
                problems.Add($"{which} has the help key {help}, which does not name its sheet, as Sheet/Key");
            }
            else if (!names.HasKey(help))
            {
                problems.Add($"{which} has the help key {help}, which no sheet holds");
            }
        }

        private static void CheckValue(string where, string element, string path, bool readsHash, LayoutNames names, List<string> problems)
        {
            if (string.IsNullOrEmpty(path))
            {
                problems.Add($"{where}: a {element} names no variable");
            }
            else if (!names.TryGetPersistentWidth(path, out VariableWidth width))
            {
                problems.Add($"{where}: a {element} shows {path}, which is no persistent value of the variable map: name one as the map lists it — Money, Items[3], Party[0].Level, or Party.Level for a single record");
            }
            else if (readsHash && width != VariableWidth.ULong)
            {
                problems.Add($"{where}: a {element} shows {path}, a {width}, but reads a localisation key's hash, a ULong");
            }
        }

        // By its name, or its place among the layout's entries; not its text, which a binding writes and the layout may not
        // hold.
        private static string Describe(MenuEntry entry, int index)
        {
            return string.IsNullOrEmpty(entry.name) ? $"{index + 1}" : $"'{entry.name}'";
        }
    }
}
