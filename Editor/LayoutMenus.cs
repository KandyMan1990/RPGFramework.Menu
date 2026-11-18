using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// The menus a layout is the layout of, by the project's Menu UI Providers — more than one where menus share it, as
    /// Save and Load do.
    /// </summary>
    internal static class LayoutMenus
    {
        internal static List<string> Of(VisualTreeAsset layout)
        {
            List<string> menus = new List<string>();
            string[]     guids = AssetDatabase.FindAssets("t:" + nameof(MenuUIProvider));

            for (int i = 0; i < guids.Length; i++)
            {
                MenuUIProvider     provider = AssetDatabase.LoadAssetAtPath<MenuUIProvider>(AssetDatabase.GUIDToAssetPath(guids[i]));
                SerializedProperty layouts  = new SerializedObject(provider).FindProperty("m_Layouts");

                for (int j = 0; j < layouts.arraySize; j++)
                {
                    string menu = MenuLayoutsCheck.Name(layouts, j);

                    if (MenuLayoutsCheck.Layout(layouts, j) == layout && !menus.Contains(menu))
                    {
                        menus.Add(menu);
                    }
                }
            }

            return menus;
        }
    }
}
