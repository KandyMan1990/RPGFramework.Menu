using System;
using RPGFramework.Menu.SubMenus;
using UnityEditor;

namespace RPGFramework.Menu.Editor
{
    /// <summary>
    /// The class a declared menu's UI is, found from the types, as only the game's installer binds it: the class bound
    /// to the menu's interface gives its UI's interface (its <c>Menu&lt;TMenuUI&gt;</c>), and the class implementing
    /// that is the UI. A game's own class is taken over the framework's at either step, as binding one is how a game
    /// replaces a menu or its UI; with two of a game's own, which is bound cannot be told, so none is.
    /// </summary>
    internal static class MenuUITypes
    {
        /// <summary>The UI of the menu bound to <paramref name="menu" />, its interface; null when it cannot be told.</summary>
        internal static Type Find(Type menu)
        {
            Type menuClass   = Choose(ClassesOf(menu));
            Type uiInterface = menuClass == null ? null : UIInterfaceOf(menuClass);

            return uiInterface == null ? null : Choose(ClassesOf(uiInterface));
        }

        /// <summary>The interface of the UI a menu's class drives, or null for a menu not built on <c>Menu&lt;TMenuUI&gt;</c>.</summary>
        internal static Type UIInterfaceOf(Type menuClass)
        {
            for (Type type = menuClass; type != null; type = type.BaseType)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Menu<>))
                {
                    return type.GetGenericArguments()[0];
                }
            }

            return null;
        }

        /// <summary>
        /// The class bound among <paramref name="candidates" />: the game's own if it has one, else the framework's; null
        /// for none, or for more than one either way.
        /// </summary>
        internal static Type Choose(Type[] candidates)
        {
            Type framework  = null;
            Type game       = null;
            int  frameworks = 0;
            int  games      = 0;

            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i].Assembly == typeof(MenuUIProvider).Assembly)
                {
                    framework = candidates[i];
                    frameworks++;
                }
                else
                {
                    game = candidates[i];
                    games++;
                }
            }

            if (games > 0)
            {
                return games == 1 ? game : null;
            }

            return frameworks == 1 ? framework : null;
        }

        private static Type[] ClassesOf(Type type)
        {
            TypeCache.TypeCollection derived = TypeCache.GetTypesDerivedFrom(type);
            int                      count   = 0;

            for (int i = 0; i < derived.Count; i++)
            {
                count += derived[i].IsClass && !derived[i].IsAbstract ? 1 : 0;
            }

            Type[] classes = new Type[count];
            int    next    = 0;

            for (int i = 0; i < derived.Count; i++)
            {
                if (derived[i].IsClass && !derived[i].IsAbstract)
                {
                    classes[next++] = derived[i];
                }
            }

            return classes;
        }
    }
}
