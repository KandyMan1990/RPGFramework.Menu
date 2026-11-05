#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RPGFramework.Hashing;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    /// <summary>
    /// Authoring only. The menus a game added to its <see cref="MenuDatabase" /> in code, which the editor cannot see
    /// before the game runs, against the names they are declared by and the layouts the UI provider holds: checked as the
    /// menu module enters in play mode, so a menu added but never declared, or added under another menu's id, is found
    /// before it is opened.
    /// </summary>
    internal static class MenuDatabaseCheck
    {
        internal static void Log(IMenuDatabase database, IMenuUIProvider provider)
        {
            List<string> problems = new List<string>();

            Check(database, provider, DeclaredMenus.FindAll(), problems);

            if (problems.Count > 0)
            {
                Debug.LogError($"The menu database has {problems.Count} problem(s):\n  {string.Join("\n  ", problems)}");
            }
        }

        // A database of the game's own cannot list its menus, so only the framework's is checked. The Menu UI Provider
        // asset's layouts are checked against the declared names as its scene loads, so here only a provider of the
        // game's own is asked for each menu's.
        internal static void Check(IMenuDatabase database, IMenuUIProvider provider, DeclaredMenu[] declared, List<string> problems)
        {
            if (database is not MenuDatabase menus)
            {
                return;
            }

            bool askProvider = provider is not MenuUIProvider;

            foreach (KeyValuePair<ulong, Type> menu in menus.Menus)
            {
                string name = NameOf(menu.Value, declared);

                if (name == null)
                {
                    problems.Add($"{menu.Value.Name} is in the menu database but declares no name: put [MenuName] on it, naming the menu as its id does, so its layout is checked before it opens");
                }
                else if (Fnv1a64.Hash(name) != menu.Key)
                {
                    problems.Add($"{menu.Value.Name} is declared as '{name}' but added to the menu database under another id: add it under the hash of '{name}'");
                }

                if (askProvider && !HasLayout(provider, menu.Key))
                {
                    problems.Add($"The menus' UI provider has no layout for {menu.Value.Name}{(name != null ? $" ('{name}')" : string.Empty)}");
                }
            }
        }

        private static string NameOf(Type menu, DeclaredMenu[] declared)
        {
            for (int i = 0; i < declared.Length; i++)
            {
                if (declared[i].Menu == menu)
                {
                    return declared[i].Name;
                }
            }

            return null;
        }

        private static bool HasLayout(IMenuUIProvider provider, ulong menuId)
        {
            try
            {
                VisualTreeAsset layout = provider.GetLayout(menuId);

                return layout != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
#endif
