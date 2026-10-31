using System;
using System.Collections.Generic;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class SaveSlotMenuUI : MenuUI<ISaveSlotMenuUI>, ISaveSlotMenuUI
    {
        private const float UNAVAILABLE_OPACITY = 0.5f;

        event Action<int> ISaveSlotMenuUI.OnSlotChosen
        {
            add => m_OnSlotChosen += value;
            remove => m_OnSlotChosen -= value;
        }

        event Action ISaveSlotMenuUI.OnNewSaveChosen
        {
            add => m_OnNewSaveChosen += value;
            remove => m_OnNewSaveChosen -= value;
        }

        event Action<bool> ISaveSlotMenuUI.OnQuestionAnswered
        {
            add => m_OnQuestionAnswered += value;
            remove => m_OnQuestionAnswered -= value;
        }

        private event Action<int>  m_OnSlotChosen;
        private event Action       m_OnNewSaveChosen;
        private event Action<bool> m_OnQuestionAnswered;

        private RPGUIButton[]        m_Rows;
        private ISaveSlotElement[][] m_SlotElements;

        private Label         m_TitleLabel;
        private ScrollView    m_SlotsScrollView;
        private VisualElement m_QuestionPanel;
        private Label         m_QuestionLabel;
        private RPGUIButton   m_YesBtn;
        private RPGUIButton   m_NoBtn;

        private bool                       m_Saving;
        private IReadOnlyList<SavePreview> m_Slots;
        private VisualElement              m_RowAskedAbout;

        private int FirstSlotRow => m_Saving ? 1 : 0;

        protected override VisualElement GetDefaultFocusedElement() => m_Rows.Length > 0 ? m_Rows[0] : null;

        public SaveSlotMenuUI(ISaveMenuLocalisationArgs localisationArgs,
                              IMenuUIProvider           uiProvider,
                              ILocalisationService      localisationService) : base(localisationArgs, uiProvider, localisationService)
        {
            m_Rows         = Array.Empty<RPGUIButton>();
            m_SlotElements = Array.Empty<ISaveSlotElement[]>();
            m_Slots        = Array.Empty<SavePreview>();
        }

        protected override void HookupUI()
        {
            m_TitleLabel      = m_UIInstance.Q<Label>("TitleLabel");
            m_SlotsScrollView = m_UIInstance.Q<ScrollView>("SlotsScrollView");
            m_QuestionPanel   = m_UIInstance.Q<VisualElement>("QuestionPanel");
            m_QuestionLabel   = m_UIInstance.Q<Label>("QuestionLabel");
            m_YesBtn          = m_UIInstance.Q<RPGUIButton>("YesBtn");
            m_NoBtn           = m_UIInstance.Q<RPGUIButton>("NoBtn");

            m_QuestionPanel.style.display = DisplayStyle.None;

            RPGUIButton.LinkRow(new RPGUIButton[] { m_YesBtn, m_NoBtn }, true);

            m_YesBtn.Chosen += OnYesChosen;
            m_NoBtn.Chosen  += OnNoChosen;
        }

        protected override void LocaliseUI()
        {
            ISaveMenuLocalisationArgs args = (ISaveMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text = m_LocalisationService.Get(m_Saving ? args.SaveTitle : args.LoadTitle);
            m_YesBtn.text     = m_LocalisationService.Get(args.Yes);
            m_NoBtn.text      = m_LocalisationService.Get(args.No);

            if (m_Saving && m_Rows.Length > 0)
            {
                m_Rows[0].text = m_LocalisationService.Get(args.NewSave);
            }

            for (int i = 0; i < m_SlotElements.Length; i++)
            {
                ISaveSlotElement[] elements = m_SlotElements[i];

                for (int j = 0; j < elements.Length; j++)
                {
                    elements[j].Show(m_Slots[i], m_LocalisationService);
                }
            }
        }

        void ISaveSlotMenuUI.SetSaving(bool saving)
        {
            m_Saving = saving;
        }

        void ISaveSlotMenuUI.SetSlots(IReadOnlyList<SavePreview> slots, int focusIndex)
        {
            m_Slots = slots;

            m_SlotsScrollView.Clear();
            m_Rows         = new RPGUIButton[FirstSlotRow + slots.Count];
            m_SlotElements = new ISaveSlotElement[slots.Count][];

            if (m_Saving)
            {
                AddRow(0, () => m_OnNewSaveChosen?.Invoke());
            }

            for (int i = 0; i < slots.Count; i++)
            {
                int index = i;

                RPGUIButton row = AddRow(FirstSlotRow + i, () => m_OnSlotChosen?.Invoke(index));

                m_SlotElements[i] = AddSlotLayout(row);

                // Still focusable when it cannot be loaded, so choosing it can say no.
                if (!m_Saving && !slots[i].CanLoad)
                {
                    row.style.opacity = UNAVAILABLE_OPACITY;
                }
            }

            RPGUIButton.LinkColumn(m_Rows, false);

            LocaliseUI();

            if (m_Rows.Length > 0)
            {
                m_Rows[focusIndex >= 0 ? FirstSlotRow + focusIndex : 0].Focus();
            }
        }

        int ISaveSlotMenuUI.GetFocusedSlot()
        {
            int row  = Array.IndexOf(m_Rows, m_UIInstance.focusController.focusedElement as RPGUIButton);
            int slot = row >= FirstSlotRow ? row - FirstSlotRow : -1;

            return slot;
        }

        void ISaveSlotMenuUI.AskToOverwrite()
        {
            Ask(((ISaveMenuLocalisationArgs)m_LocalisationArgs).OverwriteQuestion);
        }

        void ISaveSlotMenuUI.AskToDelete()
        {
            Ask(((ISaveMenuLocalisationArgs)m_LocalisationArgs).DeleteQuestion);
        }

        void ISaveSlotMenuUI.CloseQuestion()
        {
            CloseQuestion();
        }

        private void Ask(string questionKey)
        {
            m_RowAskedAbout = (VisualElement)m_UIInstance.focusController.focusedElement;

            m_QuestionLabel.text          = m_LocalisationService.Get(questionKey);
            m_QuestionPanel.style.display = DisplayStyle.Flex;

            m_NoBtn.Focus();
        }

        private void CloseQuestion()
        {
            m_QuestionPanel.style.display = DisplayStyle.None;

            m_RowAskedAbout?.Focus();
            m_RowAskedAbout = null;
        }

        private RPGUIButton AddRow(int index, Action choose)
        {
            RPGUIButton row = new RPGUIButton { focusable = true };

            row.Chosen += choose;
            row.RegisterCallback<FocusInEvent>(_ => m_SlotsScrollView.ScrollTo(row));

            m_Rows[index] = row;
            m_SlotsScrollView.Add(row);

            return row;
        }

        private ISaveSlotElement[] AddSlotLayout(RPGUIButton row)
        {
            TemplateContainer layout = m_UIProvider.SaveSlot.Instantiate();

            row.text = string.Empty;
            row.Add(layout);

            List<VisualElement> descendants = layout.Query<VisualElement>().ToList();
            int                 count       = 0;

            for (int i = 0; i < descendants.Count; i++)
            {
                if (descendants[i] is ISaveSlotElement)
                {
                    count++;
                }
            }

            ISaveSlotElement[] elements = new ISaveSlotElement[count];
            int                next     = 0;

            for (int i = 0; i < descendants.Count; i++)
            {
                if (descendants[i] is ISaveSlotElement element)
                {
                    elements[next++] = element;
                }
            }

            return elements;
        }

        private void OnYesChosen()
        {
            Answer(true);
        }

        private void OnNoChosen()
        {
            Answer(false);
        }

        private void Answer(bool yes)
        {
            CloseQuestion();

            m_OnQuestionAnswered?.Invoke(yes);
        }
    }
}