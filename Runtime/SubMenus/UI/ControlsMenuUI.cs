using System;
using System.Collections.Generic;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using Unity.Mathematics;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class ControlsMenuUI : MenuUI<IControlsMenuUI>, IControlsMenuUI
    {
        private const string RESET_BUTTON = "ResetBtn";
        private const string PROMPT       = "Prompt";

        event Action<int> IControlsMenuUI.OnControlChosen
        {
            add => m_OnControlChosen += value;
            remove => m_OnControlChosen -= value;
        }

        event Action IControlsMenuUI.OnResetPressed
        {
            add => m_OnResetPressed += value;
            remove => m_OnResetPressed -= value;
        }

        int IControlsMenuUI.ButtonCount => m_Buttons.Length;

        IReadOnlyList<RebindableControl> IControlsMenuUI.Controls => m_Controls;

        private event Action<int> m_OnControlChosen;
        private event Action      m_OnResetPressed;

        private RebindableControl[] m_Controls;
        private RPGUIButton[]       m_Buttons;
        private int[]               m_ButtonRows;
        private RPGUIButton         m_ResetBtn;
        private VisualElement       m_Prompt;

        public ControlsMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
            m_Controls   = Array.Empty<RebindableControl>();
            m_Buttons    = Array.Empty<RPGUIButton>();
            m_ButtonRows = Array.Empty<int>();
        }

        protected override VisualElement GetDefaultFocusedElement() => m_Buttons.Length > 0 ? m_Buttons[0] : m_ResetBtn;

        RebindableControl IControlsMenuUI.GetControl(int button) => m_Controls[m_ButtonRows[button]];

        string IControlsMenuUI.GetScheme(int button) => m_Buttons[button].name;

        void IControlsMenuUI.SetControlName(int button, string name)
        {
            m_Buttons[button].text = name;
        }

        void IControlsMenuUI.ShowPrompt(bool shown)
        {
            m_Prompt.style.visibility = shown ? Visibility.Visible : Visibility.Hidden;
        }

        protected override void HookupUI()
        {
            List<ControlRow> rows  = m_UIInstance.Query<ControlRow>().ToList();
            RPGUIButton[][]  cells = new RPGUIButton[rows.Count][];
            int              count = 0;

            m_Controls = new RebindableControl[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                m_Controls[i] =  rows[i].ToControl();
                cells[i]      =  rows[i].Query<RPGUIButton>().ToList().ToArray();
                count         += cells[i].Length;
            }

            m_Buttons    = new RPGUIButton[count];
            m_ButtonRows = new int[count];

            int button = 0;

            for (int row = 0; row < cells.Length; row++)
            {
                for (int i = 0; i < cells[row].Length; i++)
                {
                    int chosen = button;

                    m_Buttons[button]        =  cells[row][i];
                    m_ButtonRows[button]     =  row;
                    m_Buttons[button].Chosen += () => m_OnControlChosen?.Invoke(chosen);

                    button++;
                }

                RPGUIButton.LinkRow(cells[row], false);
            }

            m_ResetBtn        =  m_UIInstance.Q<RPGUIButton>(RESET_BUTTON);
            m_ResetBtn.Chosen += () => m_OnResetPressed?.Invoke();

            m_Prompt                  = m_UIInstance.Q(PROMPT);
            m_Prompt.style.visibility = Visibility.Hidden;

            LinkColumns(cells);
        }

        // Each column runs down the rows that have a button in it, a direction's having none for a pad, to Reset.
        private void LinkColumns(RPGUIButton[][] cells)
        {
            int columns = 0;

            for (int row = 0; row < cells.Length; row++)
            {
                columns = math.max(columns, cells[row].Length);
            }

            for (int column = 0; column < columns; column++)
            {
                int length = 1;

                for (int row = 0; row < cells.Length; row++)
                {
                    length += column < cells[row].Length ? 1 : 0;
                }

                RPGUIButton[] buttons = new RPGUIButton[length];
                int           next    = 0;

                for (int row = 0; row < cells.Length; row++)
                {
                    if (column < cells[row].Length)
                    {
                        buttons[next++] = cells[row][column];
                    }
                }

                buttons[next] = m_ResetBtn;

                RPGUIButton.LinkColumn(buttons, true);
            }
        }
    }
}
