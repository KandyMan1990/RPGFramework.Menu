using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace RPGFramework.Menu.Editor
{
    /// <summary>A command a menu declares with <see cref="MenuCommandAttribute" />, and the name of the menu it is.</summary>
    internal readonly struct DeclaredMenuCommand
    {
        internal readonly string Name;
        internal readonly string Menu;

        internal DeclaredMenuCommand(string name, string menu)
        {
            Name = name;
            Menu = menu;
        }
    }

    /// <summary>Every command declared with a <see cref="MenuCommandAttribute" />, ordered by menu, then by name.</summary>
    internal static class DeclaredMenuCommands
    {
        // Counting them first would take the same walk over each type's attributes, so they are listed as found.
        internal static DeclaredMenuCommand[] FindAll()
        {
            TypeCache.TypeCollection  types    = TypeCache.GetTypesWithAttribute<MenuCommandAttribute>();
            List<DeclaredMenuCommand> commands = new List<DeclaredMenuCommand>();

            for (int i = 0; i < types.Count; i++)
            {
                MenuNameAttribute menu = types[i].GetCustomAttribute<MenuNameAttribute>(false);
                string            name = menu?.Name ?? types[i].Name;

                foreach (MenuCommandAttribute command in types[i].GetCustomAttributes<MenuCommandAttribute>(false))
                {
                    commands.Add(new DeclaredMenuCommand(command.Name ?? string.Empty, name));
                }
            }

            commands.Sort((a, b) =>
            {
                int byMenu = string.CompareOrdinal(a.Menu, b.Menu);

                return byMenu != 0 ? byMenu : string.CompareOrdinal(a.Name, b.Name);
            });

            return commands.ToArray();
        }
    }
}
