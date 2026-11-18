using System.Collections.Generic;
using RPGFramework.Menu.UI;
using UnityEditor;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Editor
{
    /// <summary>An entry's <c>opens</c>, picked from the declared menus.</summary>
    [CustomPropertyDrawer(typeof(DeclaredMenuNameAttribute))]
    internal sealed class DeclaredMenuNameDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            DeclaredMenu[] menus = DeclaredMenus.FindAll();
            string[]       names = new string[menus.Length];

            for (int i = 0; i < menus.Length; i++)
            {
                names[i] = menus[i].Name;
            }

            return NameDropdown.Create(property, names, name => name);
        }
    }

    /// <summary>
    /// An entry's <c>command</c>, picked from the commands of the menus whose layout the UI Builder has open, so none is
    /// offered that its menu does not carry out — none at all for a layout no menu has yet. Every declared command, each
    /// shown with its menu, only should the open layout not be found.
    /// </summary>
    [CustomPropertyDrawer(typeof(DeclaredMenuCommandAttribute))]
    internal sealed class DeclaredMenuCommandDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            DeclaredMenuCommand[] commands = Offered(DeclaredMenuCommands.FindAll());
            string[]              names    = new string[commands.Length];

            for (int i = 0; i < commands.Length; i++)
            {
                names[i] = commands[i].Name;
            }

            return NameDropdown.Create(property, names, name => Label(name, commands));
        }

        private static DeclaredMenuCommand[] Offered(DeclaredMenuCommand[] commands)
        {
            VisualTreeAsset layout = UIBuilderDocument.Open();

            if (layout == null)
            {
                return commands;
            }

            List<string> menus   = LayoutMenus.Of(layout);
            int          count   = 0;
            bool[]       offered = new bool[commands.Length];

            for (int i = 0; i < commands.Length; i++)
            {
                offered[i] =  menus.Contains(commands[i].Menu);
                count      += offered[i] ? 1 : 0;
            }

            DeclaredMenuCommand[] ofLayout = new DeclaredMenuCommand[count];
            int                   next     = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                if (offered[i])
                {
                    ofLayout[next++] = commands[i];
                }
            }

            return ofLayout;
        }

        private static string Label(string name, DeclaredMenuCommand[] commands)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].Name == name)
                {
                    return $"{name} ({commands[i].Menu})";
                }
            }

            return name;
        }
    }
}
