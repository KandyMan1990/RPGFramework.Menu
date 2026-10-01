using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SaveData;
using RPGFramework.Menu.SharedTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGFramework.Menu.SubMenus
{
    public abstract class SaveSlotMenu : Menu<ISaveSlotMenuUI>
    {
        private enum Question
        {
            None,
            Overwrite,
            Delete
        }

        protected override bool m_HidePreviousUiOnSuspend => true;

        protected readonly ISaveDataService m_SaveDataService;
        protected readonly List<string>     m_Files = new List<string>();

        private readonly List<bool> m_CanLoad = new List<bool>();

        private Question m_Question;
        private string   m_QuestionFile;

        protected abstract bool IsSaving { get; }

        protected SaveSlotMenu(ISaveSlotMenuUI    saveSlotMenuUI,
                               IInputRouter       inputRouter,
                               IMenuModule        menuModule,
                               IAudioIntentPlayer audioIntentPlayer,
                               ISaveDataService   saveDataService) : base(saveSlotMenuUI, inputRouter, menuModule, audioIntentPlayer)
        {
            m_SaveDataService = saveDataService;
        }

        protected override Task OnEnterAsync(Dictionary<string, object> args)
        {
            m_MenuUI.SetSaving(IsSaving);

            return base.OnEnterAsync(args);
        }

        protected override Task OnEnterComplete()
        {
            ShowSlots(m_SaveDataService.GetCurrentSaveFileName());

            return base.OnEnterComplete();
        }

        protected override void RegisterCallbacks()
        {
            m_MenuUI.OnSlotChosen       += OnSlotChosen;
            m_MenuUI.OnNewSaveChosen    += OnNewSaveChosen;
            m_MenuUI.OnQuestionAnswered += OnQuestionAnswered;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnQuestionAnswered -= OnQuestionAnswered;
            m_MenuUI.OnNewSaveChosen    -= OnNewSaveChosen;
            m_MenuUI.OnSlotChosen       -= OnSlotChosen;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            switch (slot)
            {
                case ControlSlot.Secondary:
                    OnBack();
                    break;
                case ControlSlot.Tertiary:
                    OnDeletePressed();
                    break;
            }

            return true;
        }

        protected abstract void OnSlotChosen(int index);

        protected virtual void OnNewSaveChosen()
        {
        }

        protected virtual void OnOverwriteConfirmed(string file)
        {
        }

        protected void AskToOverwrite(int index)
        {
            m_Question     = Question.Overwrite;
            m_QuestionFile = m_Files[index];

            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_MenuUI.AskToOverwrite();
        }

        /// <summary>
        /// A save written by a newer version of the game holds variables this one does not know, and a damaged one cannot
        /// be read, so neither can be loaded.
        /// </summary>
        protected bool CanLoad(int index)
        {
            bool canLoad = m_CanLoad[index];

            return canLoad;
        }

        protected void ShowSlots(string focusFile)
        {
            List<SavePreview> previews = new List<SavePreview>();

            foreach (string file in m_SaveDataService.GetListOfSaveFiles())
            {
                previews.Add(m_SaveDataService.ReadPreview(file));
            }

            previews.Sort((a, b) => b.LastWritten.CompareTo(a.LastWritten));

            List<SaveSlotInfo> slots      = new List<SaveSlotInfo>(previews.Count);
            int                focusIndex = -1;

            m_Files.Clear();
            m_CanLoad.Clear();

            foreach (SavePreview preview in previews)
            {
                if (preview.FileName == focusFile)
                {
                    focusIndex = m_Files.Count;
                }

                m_Files.Add(preview.FileName);
                m_CanLoad.Add(preview.CanLoad);
                slots.Add(new SaveSlotInfo(preview.Read<ulong>(CoreVariables.LOCATION_NAME), preview.Read<uint>(CoreVariables.PLAY_TIME), preview.LastWritten, preview.IsFromNewerVersion, preview.IsDamaged));
            }

            m_MenuUI.SetSlots(slots, focusIndex);
        }

        private void OnBack()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);

            if (m_Question == Question.None)
            {
                m_MenuModule.PopMenu().FireAndForget();
                return;
            }

            m_Question     = Question.None;
            m_QuestionFile = null;

            m_MenuUI.CloseQuestion();
        }

        private void OnDeletePressed()
        {
            if (m_Question != Question.None)
            {
                return;
            }

            int index = m_MenuUI.GetFocusedSlot();

            if (index < 0)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_Question     = Question.Delete;
            m_QuestionFile = m_Files[index];

            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_MenuUI.AskToDelete();
        }

        private void OnQuestionAnswered(bool yes)
        {
            Question question = m_Question;
            string   file     = m_QuestionFile;

            m_Question     = Question.None;
            m_QuestionFile = null;

            if (!yes)
            {
                m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
                return;
            }

            if (question == Question.Delete)
            {
                Delete(file);
                return;
            }

            OnOverwriteConfirmed(file);
        }

        private void Delete(string file)
        {
            int index = m_Files.IndexOf(file);

            string focusFile = index + 1 < m_Files.Count ? m_Files[index + 1]
                             : index > 0                 ? m_Files[index - 1]
                                                         : null;

            m_SaveDataService.DeleteSave(file);

            m_AudioIntentPlayer.Play(AudioIntent.Confirm, AudioContext.Menu);

            ShowSlots(focusFile);
        }
    }
}
