using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class SaveMenu : SaveSlotMenu, ISaveMenu
    {
        protected override bool IsSaving => true;

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
            AskToOverwrite(index);
        }

        protected override void OnOverwriteConfirmed(string file)
        {
            Save(file);
        }

        private void Save(string file)
        {
            m_SaveDataService.CommitSave(file);

            m_AudioIntentPlayer.Play(AudioIntent.SaveGame, AudioContext.Menu);

            ShowSlots(file);
        }
    }
}
