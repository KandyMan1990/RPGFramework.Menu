#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace RPGFramework.Menu
{
    /// <summary>Authoring only. A menu the editor knows of, by the <see cref="MenuNameAttribute" /> on its type.</summary>
    internal readonly struct DeclaredMenu
    {
        internal readonly string Name;
        internal readonly Type   Menu;

        internal DeclaredMenu(string name, Type menu)
        {
            Name = name;
            Menu = menu;
        }
    }

    /// <summary>Authoring only. Every menu declared with a <see cref="MenuNameAttribute" />, ordered by name.</summary>
    internal static class DeclaredMenus
    {
        internal static DeclaredMenu[] FindAll()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesWithAttribute<MenuNameAttribute>();
            DeclaredMenu[]           menus = new DeclaredMenu[types.Count];

            for (int i = 0; i < types.Count; i++)
            {
                menus[i] = new DeclaredMenu(types[i].GetCustomAttribute<MenuNameAttribute>(false).Name ?? string.Empty, types[i]);
            }

            Array.Sort(menus, (a, b) => string.CompareOrdinal(a.Name, b.Name));

            return menus;
        }
    }
}
#endif
