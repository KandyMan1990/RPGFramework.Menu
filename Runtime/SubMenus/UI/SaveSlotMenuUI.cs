using System;
using System.Collections.Generic;
using RPGFramework.Core.SaveData;
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

        private VisualElement[]      m_Rows;
        private ISaveSlotElement[][] m_SlotElements;

        private Label         m_SaveTitleLabel;
        private Label         m_LoadTitleLabel;
        private ScrollView    m_SlotsScrollView;
        private VisualElement m_NewSaveRow;
        private VisualElement m_QuestionPanel;
        private Label         m_OverwriteQuestionLabel;
        private Label         m_DeleteQuestionLabel;
        private VisualElement m_YesBtn;
        private VisualElement m_NoBtn;

        private bool                       m_Saving;
        private IReadOnlyList<SavePreview> m_Slots;
        private VisualElement              m_RowAskedAbout;

        private int FirstSlotRow => m_Saving ? 1 : 0;

        protected override VisualElement GetDefaultFocusedElement() => m_Rows.Length > 0 ? m_Rows[0] : null;

        public SaveSlotMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
            m_Rows         = Array.Empty<VisualElement>();
            m_SlotElements = Array.Empty<ISaveSlotElement[]>();
            m_Slots        = Array.Empty<SavePreview>();
        }

        protected override void HookupUI()
        {
            m_SaveTitleLabel         = m_UIInstance.Q<Label>("SaveTitleLabel");
            m_LoadTitleLabel         = m_UIInstance.Q<Label>("LoadTitleLabel");
            m_SlotsScrollView        = m_UIInstance.Q<ScrollView>("SlotsScrollView");
            m_NewSaveRow             = m_UIInstance.Q("NewSaveBtn");
            m_QuestionPanel          = m_UIInstance.Q<VisualElement>("QuestionPanel");
            m_OverwriteQuestionLabel = m_UIInstance.Q<Label>("OverwriteQuestionLabel");
            m_DeleteQuestionLabel    = m_UIInstance.Q<Label>("DeleteQuestionLabel");
            m_YesBtn                 = m_UIInstance.Q("YesBtn");
            m_NoBtn                  = m_UIInstance.Q("NoBtn");

            m_QuestionPanel.style.display = DisplayStyle.None;

            // The layout places it among the slots; it is put back first while saving.
            m_NewSaveRow.RemoveFromHierarchy();
            HookUpRow(m_NewSaveRow, () => m_OnNewSaveChosen?.Invoke());

            m_Selection.LinkRow(new[] { m_YesBtn, m_NoBtn }, true);

            m_Selection.RegisterChosen(m_YesBtn, OnYesChosen);
            m_Selection.RegisterChosen(m_NoBtn, OnNoChosen);
        }

        // A slot's layout is cloned per save once the slots are known, after the sheets load.
        protected override string[] GetSheetNames() =>
            new LocalisedTexts(m_UIInstance, m_UIProvider.SaveSlot.Instantiate()).SheetNames;

        protected override void LocaliseUI()
        {
            ShowIf(m_SaveTitleLabel, m_Saving);
            ShowIf(m_LoadTitleLabel, !m_Saving);

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
            m_Rows         = new VisualElement[FirstSlotRow + slots.Count];
            m_SlotElements = new ISaveSlotElement[slots.Count][];

            if (m_Saving)
            {
                PlaceRow(0, m_NewSaveRow);
            }

            for (int i = 0; i < slots.Count; i++)
            {
                int index = i;

                VisualElement row = AddRow(FirstSlotRow + i, () => m_OnSlotChosen?.Invoke(index));

                m_SlotElements[i] = AddSlotLayout(row);

                // Still focusable when it cannot be loaded, so choosing it can say no.
                if (!m_Saving && !slots[i].CanLoad)
                {
                    row.style.opacity = UNAVAILABLE_OPACITY;
                }
            }

            m_Selection.LinkColumn(m_Rows, false);

            Localise();

            if (m_Rows.Length > 0)
            {
                m_Rows[focusIndex >= 0 ? FirstSlotRow + focusIndex : 0].Focus();
            }
        }

        int ISaveSlotMenuUI.GetFocusedSlot()
        {
            int row  = Array.IndexOf(m_Rows, m_UIInstance.focusController.focusedElement as VisualElement);
            int slot = row >= FirstSlotRow ? row - FirstSlotRow : -1;

            return slot;
        }

        void ISaveSlotMenuUI.AskToOverwrite()
        {
            Ask(m_OverwriteQuestionLabel);
        }

        void ISaveSlotMenuUI.AskToDelete()
        {
            Ask(m_DeleteQuestionLabel);
        }

        void ISaveSlotMenuUI.CloseQuestion()
        {
            CloseQuestion();
        }

        private void Ask(Label question)
        {
            m_RowAskedAbout = (VisualElement)m_UIInstance.focusController.focusedElement;

            ShowIf(m_OverwriteQuestionLabel, question == m_OverwriteQuestionLabel);
            ShowIf(m_DeleteQuestionLabel, question == m_DeleteQuestionLabel);

            m_QuestionPanel.style.display = DisplayStyle.Flex;

            m_NoBtn.Focus();
        }

        private void CloseQuestion()
        {
            m_QuestionPanel.style.display = DisplayStyle.None;

            m_RowAskedAbout?.Focus();
            m_RowAskedAbout = null;
        }

        private static void ShowIf(VisualElement element, bool shown)
        {
            element.style.display = shown ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private VisualElement AddRow(int index, Action choose)
        {
            VisualElement row = new VisualElement();

            HookUpRow(row, choose);
            PlaceRow(index, row);

            return row;
        }

        private void HookUpRow(VisualElement row, Action choose)
        {
            m_Selection.RegisterChosen(row, choose);
            row.RegisterCallback<FocusInEvent>(_ => m_SlotsScrollView.ScrollTo(row));
        }

        private void PlaceRow(int index, VisualElement row)
        {
            m_Rows[index] = row;
            m_SlotsScrollView.Add(row);
        }

        private ISaveSlotElement[] AddSlotLayout(VisualElement row)
        {
            TemplateContainer layout = m_UIProvider.SaveSlot.Instantiate();

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
