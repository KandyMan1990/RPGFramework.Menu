using System;
using RPGFramework.Core.Input;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// A row of the Controls page: one control the player can rebind, the actions it is in — <c>Player/Confirm,
    /// UI/Submit</c>, rebound as one — and, for a direction, the part it is, such as <c>up</c>. Its buttons are named
    /// for the control schemes they rebind it in, <c>Keyboard&amp;Mouse</c> or <c>Gamepad</c>.
    /// </summary>
    [UxmlElement]
    public sealed partial class ControlRow : VisualElement
    {
        private static readonly char[] m_Separators = { ',' };

        /// <summary>The actions the control is in, each <c>Map/Action</c>, separated by commas.</summary>
        [UxmlAttribute]
        public string Actions { get; set; }

        /// <summary>The part of a composite the control is, such as <c>up</c>; empty for a button.</summary>
        [UxmlAttribute]
        public string Part { get; set; }

        internal RebindableControl ToControl()
        {
            string[] actions = (Actions ?? string.Empty).Split(m_Separators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i] = actions[i].Trim();
            }

            return new RebindableControl(actions, string.IsNullOrEmpty(Part) ? null : Part);
        }
    }
}
