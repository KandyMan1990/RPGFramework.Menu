using System.Collections.Generic;
using RPGFramework.Menu.Editor;
using RPGFramework.Menu.SharedTypes.Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests
{
    /// <summary>
    /// A Menu UI Provider as the Menu Sample's is: a layout for each of the framework's menus, under the names they are
    /// declared by, and a save slot, authored through the inspector's serialized properties.
    /// </summary>
    internal sealed class MenusToCheck
    {
        internal const string STATUS = "Status";

        internal static readonly DeclaredMenu[] Framework =
        {
            new DeclaredMenu(MenuIds.BEGIN, typeof(IBeginMenu)),
            new DeclaredMenu(MenuIds.CONFIG, typeof(IConfigMenu)),
            new DeclaredMenu(MenuIds.CONTROLS, typeof(IControlsMenu)),
            new DeclaredMenu(MenuIds.LANGUAGE, typeof(ILanguageMenu)),
            new DeclaredMenu(MenuIds.LOAD, typeof(ILoadMenu)),
            new DeclaredMenu(MenuIds.PARTY, typeof(IPartyMenu)),
            new DeclaredMenu(MenuIds.SAVE, typeof(ISaveMenu))
        };

        private readonly MenuUIProvider  m_Provider;
        private readonly VisualTreeAsset m_Layout;

        internal MenusToCheck()
        {
            m_Provider = ScriptableObject.CreateInstance<MenuUIProvider>();
            m_Layout   = ScriptableObject.CreateInstance<VisualTreeAsset>();

            m_Provider.name = "Menu UI Provider";

            SerializedObject serialized = new SerializedObject(m_Provider);

            serialized.FindProperty("m_SaveSlot").objectReferenceValue = m_Layout;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            for (int i = 0; i < Framework.Length; i++)
            {
                AddLayout(Framework[i].Name);
            }
        }

        internal MenuUIProvider Provider => m_Provider;

        /// <summary>The layout every menu, and the save slot, is given.</summary>
        internal VisualTreeAsset Layout => m_Layout;

        internal void AddLayout(string menu)
        {
            SerializedObject   serialized = new SerializedObject(m_Provider);
            SerializedProperty layouts    = serialized.FindProperty("m_Layouts");
            int                index      = layouts.arraySize;

            layouts.arraySize++;
            layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Menu").stringValue           = menu;
            layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Layout").objectReferenceValue = m_Layout;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        internal void RemoveLayout(string menu)
        {
            SerializedObject   serialized = new SerializedObject(m_Provider);
            SerializedProperty layouts    = serialized.FindProperty("m_Layouts");

            layouts.DeleteArrayElementAtIndex(MenuLayoutsCheck.IndexOf(layouts, menu));
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        internal void RemoveSaveSlot()
        {
            SerializedObject serialized = new SerializedObject(m_Provider);

            serialized.FindProperty("m_SaveSlot").objectReferenceValue = null;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>The provider's problems, against the framework's menus and any more declared.</summary>
        internal List<string> CheckLayouts(params DeclaredMenu[] more)
        {
            List<string> problems = new List<string>();

            MenuLayoutsCheck.Check(m_Provider, With(more), problems);

            return problems;
        }

        /// <summary>The database's problems, against the framework's menus and any more declared.</summary>
        internal static List<string> CheckDatabase(IMenuDatabase database, IMenuUIProvider provider, params DeclaredMenu[] more)
        {
            List<string> problems = new List<string>();

            MenuDatabaseCheck.Check(database, provider, With(more), problems);

            return problems;
        }

        internal void Destroy()
        {
            Object.DestroyImmediate(m_Provider);
            Object.DestroyImmediate(m_Layout);
        }

        private static DeclaredMenu[] With(DeclaredMenu[] more)
        {
            DeclaredMenu[] declared = new DeclaredMenu[Framework.Length + more.Length];

            Framework.CopyTo(declared, 0);
            more.CopyTo(declared, Framework.Length);

            return declared;
        }
    }
}
