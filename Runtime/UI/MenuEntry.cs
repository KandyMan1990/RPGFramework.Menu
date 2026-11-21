using System;
using RPGFramework.Core.Memory;
using RPGFramework.Hashing;
using RPGFramework.Localisation;
using Unity.Properties;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Anything the cursor stops on in a menu, laid out by the menu's layout: a line of text of its own, and whatever
    /// else the layout puts in it. It opens a menu, by name, or runs a command the menu carries out itself, or neither,
    /// for its menu to handle; it is greyed while any variable <see cref="When" /> names is false; and its help is the
    /// text <see cref="Help" /> names, shown in the menu's help line while the cursor is on it.
    /// </summary>
    [UxmlElement]
    public sealed partial class MenuEntry : VisualElement
    {
        private const string ENTRY_CLASS = "rpg-menu-entry";
        private const string LABEL_CLASS = "rpg-ui-label";

        /// <summary>Its text, which a localised text's binding writes.</summary>
        [UxmlAttribute, CreateProperty]
        public string text
        {
            get => m_Label.text;
            set => m_Label.text = value;
        }

        /// <summary>The name of the menu it opens, such as <c>Save</c> or a game's own; empty for none.</summary>
        [UxmlAttribute, DeclaredMenuName]
        public string Opens { get; set; }

        /// <summary>The name of the command its menu carries out, such as the title's <c>NewGame</c>; empty for none.</summary>
        [UxmlAttribute, DeclaredMenuCommand]
        public string Command { get; set; }

        /// <summary>
        /// Bool variables, by name, the entry is greyed while any is false — comma-separated in the layout,
        /// <c>HasMenuAbility, GfUnsealed</c>; none to be offered always.
        /// </summary>
        [UxmlAttribute, VariableName(VariableWidth.Bool)]
        public string[] When { get; set; }

        /// <summary>The key of its help, <c>Sheet/Key</c>, shown in the menu's help line; empty for none.</summary>
        [UxmlAttribute, LocalisationKey]
        public string Help { get; set; }

        /// <summary>Whether the cursor starts here as the menu opens, rather than on the first entry.</summary>
        [UxmlAttribute]
        public bool DefaultFocus { get; set; }

        private readonly Label m_Label;

        public MenuEntry()
        {
            m_Label = new Label();

            AddToClassList(ENTRY_CLASS);

            m_Label.AddToClassList(LABEL_CLASS);
            m_Label.focusable   = false;
            m_Label.pickingMode = PickingMode.Ignore;

            Add(m_Label);
        }

        internal MenuChoice ToChoice()
        {
            return new MenuChoice(HashOf(Opens), HashOf(Command), When ?? Array.Empty<string>());
        }

        private static ulong HashOf(string name)
        {
            return string.IsNullOrEmpty(name) ? 0UL : Fnv1a64.Hash(name);
        }
    }
}
