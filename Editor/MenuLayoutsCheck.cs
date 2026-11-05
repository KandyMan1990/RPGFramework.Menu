using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// Whether a Menu UI Provider holds a layout for every declared menu, under the name it is declared by, and nothing
    /// else. The names are strings in code and in the asset, so nothing else would notice one wrong: the menu would
    /// throw only as it opened.
    /// </summary>
    internal static class MenuLayoutsCheck
    {
        /// <summary>The problems with every Menu UI Provider a component of the menu scene references.</summary>
        internal static void CheckScene(Scene scene, List<string> problems)
        {
            List<MenuUIProvider> providers = ProvidersIn(scene);

            if (providers.Count == 0)
            {
                return;
            }

            DeclaredMenu[] declared = DeclaredMenus.FindAll();

            for (int i = 0; i < providers.Count; i++)
            {
                Check(providers[i], declared, problems);
            }
        }

        internal static bool HoldsMenuModule(Scene scene)
        {
            GameObject[] roots = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].GetComponentInChildren<MenuModuleMonoBehaviour>(true) != null)
                {
                    return true;
                }
            }

            return false;
        }

        internal static void Check(MenuUIProvider provider, DeclaredMenu[] declared, List<string> problems)
        {
            SerializedObject   serialized = new SerializedObject(provider);
            SerializedProperty layouts    = serialized.FindProperty("m_Layouts");
            string             where      = $"'{provider.name}'";

            for (int i = 0; i < declared.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (declared[j].Name == declared[i].Name)
                    {
                        problems.Add($"'{declared[i].Name}' is declared by both {declared[j].Menu.Name} and {declared[i].Menu.Name}: give each menu a name of its own");
                    }
                }

                int found = IndexOf(layouts, declared[i].Name);

                if (found < 0 || Layout(layouts, found) == null)
                {
                    problems.Add($"{where} has no layout for {declared[i].Name} ({declared[i].Menu.Name}): give it one in its inspector");
                }
            }

            for (int i = 0; i < layouts.arraySize; i++)
            {
                string name = Name(layouts, i);

                if (IndexOf(layouts, name) < i)
                {
                    problems.Add($"{where} holds two layouts for '{name}'");
                }
                else if (!IsDeclared(name, declared))
                {
                    problems.Add($"{where} holds a layout for '{name}', which no menu declares: put [MenuName] on the menu's interface, or remove it in the inspector");
                }
            }

            if (serialized.FindProperty("m_SaveSlot").objectReferenceValue == null)
            {
                problems.Add($"{where} has no Save Slot layout, which each row of the Save and Load menus is made from");
            }
        }

        internal static bool IsDeclared(string name, DeclaredMenu[] declared)
        {
            for (int i = 0; i < declared.Length; i++)
            {
                if (declared[i].Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        internal static int IndexOf(SerializedProperty layouts, string name)
        {
            for (int i = 0; i < layouts.arraySize; i++)
            {
                if (Name(layouts, i) == name)
                {
                    return i;
                }
            }

            return -1;
        }

        internal static string Name(SerializedProperty layouts, int index)
        {
            return layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Menu").stringValue;
        }

        internal static Object Layout(SerializedProperty layouts, int index)
        {
            return layouts.GetArrayElementAtIndex(index).FindPropertyRelative("m_Layout").objectReferenceValue;
        }

        // The provider is the game's installer's to bind, so it is found by what the scene's components reference.
        private static List<MenuUIProvider> ProvidersIn(Scene scene)
        {
            List<MenuUIProvider> providers = new List<MenuUIProvider>();
            GameObject[]         roots     = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
            {
                MonoBehaviour[] components = roots[i].GetComponentsInChildren<MonoBehaviour>(true);

                for (int j = 0; j < components.Length; j++)
                {
                    if (components[j] == null)
                    {
                        continue;
                    }

                    SerializedProperty property = new SerializedObject(components[j]).GetIterator();

                    while (property.Next(true))
                    {
                        if (property.propertyType == SerializedPropertyType.ObjectReference &&
                            property.objectReferenceValue is MenuUIProvider provider &&
                            !providers.Contains(provider))
                        {
                            providers.Add(provider);
                        }
                    }
                }
            }

            return providers;
        }
    }
}
