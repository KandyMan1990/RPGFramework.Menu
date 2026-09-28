using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class SaveMenu : SaveSlotMenu, ISaveMenu
    {
        protected override bool IsSaving => true;

        private string m_PendingOverwrite;

        public SaveMenu(ISaveSlotMenuUI    saveSlotMenuUI,
                        IInputRouter       inputRouter,
                        IMenuModule        menuModule,
                        IAudioIntentPlayer audioIntentPlayer,
                        ISaveDataService   saveDataService) : base(saveSlotMenuUI, inputRouter, menuModule, audioIntentPlayer, saveDataService)
        {
        }

        protected override void OnNewSaveChosen()
        {
            Save(m_SaveDataService.GetUnusedSaveFileName());
        }

        protected override void OnSlotChosen(int index)
        {
            m_PendingOverwrite = m_Files[index];

            m_AudioIntentPlayer.Play(AudioIntent.Navigate, AudioContext.Menu);
            m_MenuUI.AskToOverwrite();
        }

        protected override void OnOverwriteAnswered(bool overwrite)
        {
            string file = m_PendingOverwrite;
            m_PendingOverwrite = null;

            if (overwrite)
            {
                Save(file);
                return;
            }

            m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
        }

        protected override void OnBack()
        {
            if (m_PendingOverwrite == null)
            {
                base.OnBack();
                return;
            }

            m_PendingOverwrite = null;

            m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
            m_MenuUI.CloseOverwriteQuestion();
        }

        private void Save(string file)
        {
            m_SaveDataService.CommitSave(file);

            m_AudioIntentPlayer.Play(AudioIntent.SaveGame, AudioContext.Menu);

            ShowSlots(file);
        }
    }
}