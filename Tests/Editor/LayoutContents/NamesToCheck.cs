using System;
using System.Collections.Generic;
using RPGFramework.Core.Memory;
using RPGFramework.Localisation.Editor;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SharedTypes.Constants;
using RPGFramework.Menu.SubMenus;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RPGFramework.Menu.Tests.LayoutContents
{
    /// <summary>
    /// A project's names as the Menu Sample's are: the framework's menus and the title's commands, a new variable map
    /// holding the variables the framework requires, and the party menu's sheet holding one help key. No menu's UI is
    /// known unless one is named, so only what a test asks for is checked of a layout's elements.
    /// </summary>
    internal sealed class NamesToCheck
    {
        internal const string HELP = "PartyMenu/SaveHelp";

        private readonly VariableMapAsset       m_Map;
        private readonly LocalisationSheetAsset m_Sheet;
        private readonly LayoutNames            m_Names;

        internal NamesToCheck() : this(null, null)
        {
        }

        /// <param name="menu">A menu whose UI is known.</param>
        /// <param name="ui">The class of its UI.</param>
        internal NamesToCheck(string menu, Type ui)
        {
            Type[] uis = new Type[MenusToCheck.Framework.Length];

            for (int i = 0; i < uis.Length; i++)
            {
                uis[i] = MenusToCheck.Framework[i].Name == menu ? ui : null;
            }

            m_Map   = ScriptableObject.CreateInstance<VariableMapAsset>();
            m_Sheet = ScriptableObject.CreateInstance<LocalisationSheetAsset>();

            m_Sheet.name = "PartyMenu";

            SerializedObject   serialized = new SerializedObject(m_Sheet);
            SerializedProperty keys       = serialized.FindProperty("m_Keys");

            keys.arraySize = 1;
            keys.GetArrayElementAtIndex(0).stringValue = HELP;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            DeclaredMenuCommand[] commands =
            {
                new DeclaredMenuCommand(BeginMenuCommands.NEW_GAME, MenuIds.BEGIN),
                new DeclaredMenuCommand(BeginMenuCommands.QUIT, MenuIds.BEGIN)
            };

            m_Names = new LayoutNames(MenusToCheck.Framework, uis, commands, new[] { m_Map }, new[] { m_Sheet });
        }

        internal LayoutNames Names => m_Names;

        /// <summary>The problems of a layout of <paramref name="menu" /> holding <paramref name="elements" />.</summary>
        internal List<string> CheckEntries(string menu, params VisualElement[] elements)
        {
            List<string> problems = new List<string>();

            MenuLayoutContentsCheck.CheckEntries(menu, menu, LayoutOf(elements), m_Names, problems);

            return problems;
        }

        /// <summary>The problems of a save slot's layout holding <paramref name="elements" />.</summary>
        internal List<string> CheckSaveSlot(params VisualElement[] elements)
        {
            List<string> problems = new List<string>();

            MenuLayoutContentsCheck.CheckSaveSlot("Slot", LayoutOf(elements), m_Names, problems);

            return problems;
        }

        internal void Destroy()
        {
            Object.DestroyImmediate(m_Map);
            Object.DestroyImmediate(m_Sheet);
        }

        private static VisualElement LayoutOf(VisualElement[] elements)
        {
            VisualElement layout = new VisualElement();

            for (int i = 0; i < elements.Length; i++)
            {
                layout.Add(elements[i]);
            }

            return layout;
        }
    }
}
