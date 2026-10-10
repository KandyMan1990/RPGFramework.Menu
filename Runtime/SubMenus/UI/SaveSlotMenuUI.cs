using System;
using System.Collections.Generic;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    public class SaveSlotMenuUI : MenuUI<ISaveSlotMenuUI>, ISaveSlotMenuUI
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

        private RPGUIButton[] m_Rows;

        private Label         m_TitleLabel;
        private ScrollView    m_SlotsScrollView;
        private VisualElement m_QuestionPanel;
        private Label         m_QuestionLabel;
        private RPGUIButton   m_YesBtn;
        private RPGUIButton   m_NoBtn;

        private bool                        m_Saving;
        private IReadOnlyList<SaveSlotInfo> m_Slots;
        private VisualElement               m_RowAskedAbout;

        private int FirstSlotRow => m_Saving ? 1 : 0;

        protected override VisualElement GetDefaultFocusedElement() => m_Rows.Length > 0 ? m_Rows[0] : null;

        public SaveSlotMenuUI(ISaveMenuLocalisationArgs localisationArgs,
                              IMenuUIProvider           uiProvider,
                              IAudioIntentPlayer        audioIntentPlayer,
                              ILocalisationService      localisationService) : base(localisationArgs, uiProvider, audioIntentPlayer, localisationService)
        {
            m_Rows  = Array.Empty<RPGUIButton>();
            m_Slots = Array.Empty<SaveSlotInfo>();
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
        }

        protected override void LocaliseUI()
        {
            ISaveMenuLocalisationArgs args = (ISaveMenuLocalisationArgs)m_LocalisationArgs;

            m_TitleLabel.text = m_LocalisationService.Get(m_Saving ? args.SaveTitle : args.LoadTitle);
            m_YesBtn.text     = m_LocalisationService.Get(args.Yes);
            m_NoBtn.text      = m_LocalisationService.Get(args.No);

            int first = FirstSlotRow;

            if (m_Saving && m_Rows.Length > 0)
            {
                m_Rows[0].text = m_LocalisationService.Get(args.NewSave);
            }

            for (int i = 0; i < m_Slots.Count && first + i < m_Rows.Length; i++)
            {
                m_Rows[first + i].text = Describe(m_Slots[i], args);
            }
        }

        protected override void RegisterCallbacks()
        {
            UIToolkitInputUtility.RegisterButtonCallbacks(m_YesBtn, OnYesBtnNavigate, OnYesBtnSubmitted, OnYesBtnClicked);
            UIToolkitInputUtility.RegisterButtonCallbacks(m_NoBtn,  OnNoBtnNavigate,  OnNoBtnSubmitted,  OnNoBtnClicked);
        }

        protected override void UnregisterCallbacks()
        {
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_NoBtn,  OnNoBtnNavigate,  OnNoBtnSubmitted,  OnNoBtnClicked);
            UIToolkitInputUtility.UnregisterButtonCallbacks(m_YesBtn, OnYesBtnNavigate, OnYesBtnSubmitted, OnYesBtnClicked);
        }

        void ISaveSlotMenuUI.SetSaving(bool saving)
        {
            m_Saving = saving;
        }

        void ISaveSlotMenuUI.SetSlots(IReadOnlyList<SaveSlotInfo> slots, int focusIndex)
        {
            m_Slots = slots;

            m_SlotsScrollView.Clear();
            m_Rows = new RPGUIButton[FirstSlotRow + slots.Count];

            if (m_Saving)
            {
                AddRow(0, () => m_OnNewSaveChosen?.Invoke());
            }

            for (int i = 0; i < slots.Count; i++)
            {
                int index = i;

                RPGUIButton row = AddRow(FirstSlotRow + i, () => m_OnSlotChosen?.Invoke(index));

                // Still focusable when it cannot be loaded, so choosing it can say no.
                if (!m_Saving && (slots[i].FromNewerVersion || slots[i].Damaged))
                {
                    row.style.opacity = UNAVAILABLE_OPACITY;
                }
            }

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

            row.RegisterCallback<ClickEvent>(_ => choose());
            row.RegisterCallback<NavigationSubmitEvent>(_ => choose());
            row.RegisterCallback<NavigationMoveEvent>(OnRowNavigate);
            row.RegisterCallback<FocusInEvent>(_ => m_SlotsScrollView.ScrollTo(row));

            m_Rows[index] = row;
            m_SlotsScrollView.Add(row);

            return row;
        }

        private void OnRowNavigate(NavigationMoveEvent evt)
        {
            RPGUIButton row   = (RPGUIButton)evt.currentTarget;
            int         index = Array.IndexOf(m_Rows, row);

            RPGUIButton up   = index > 0 ? m_Rows[index                - 1] : null;
            RPGUIButton down = index < m_Rows.Length - 1 ? m_Rows[index + 1] : null;

            if (UIToolkitInputUtility.Navigate(evt, row, up, down))
            {
                OnBtnNavigate();
            }
        }

        private string Describe(SaveSlotInfo slot, ISaveMenuLocalisationArgs args)
        {
            if (slot.Damaged)
            {
                string damaged = $"{m_LocalisationService.Get(args.Damaged)}    {slot.LastWritten:g}";

                return damaged;
            }

            string location = m_LocalisationService.TryGet(slot.LocationName, out string name) ? name : string.Empty;
            string time     = $"{slot.PlayTime / 3600}:{slot.PlayTime / 60 % 60:00}";

            string description = slot.FromNewerVersion
                                     ? $"{location}    {time}    {slot.LastWritten:g}    {m_LocalisationService.Get(args.NewerVersion)}"
                                     : $"{location}    {time}    {slot.LastWritten:g}";

            return description;
        }

        private void OnYesBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_YesBtn, left: m_NoBtn, right: m_NoBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnYesBtnSubmitted(NavigationSubmitEvent evt)
        {
            Answer(true);
        }

        private void OnYesBtnClicked(ClickEvent evt)
        {
            Answer(true);
        }

        private void OnNoBtnNavigate(NavigationMoveEvent evt)
        {
            if (UIToolkitInputUtility.Navigate(evt, m_NoBtn, left: m_YesBtn, right: m_YesBtn))
            {
                OnBtnNavigate();
            }
        }

        private void OnNoBtnSubmitted(NavigationSubmitEvent evt)
        {
            Answer(false);
        }

        private void OnNoBtnClicked(ClickEvent evt)
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