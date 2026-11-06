using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    internal sealed class SaveMenu : SaveSlotMenu, ISaveMenu
    {
        protected override bool IsSaving => true;

        public SaveMenu(ISaveSlotMenuUI    saveSlotMenuUI,
                        IInputRouter       inputRouter,
                        IMenuModule        menuModule,
                        ISfxPlayer         sfxPlayer,
                        ISystemSoundConfig systemSounds,
                        ISaveDataService   saveDataService) : base(saveSlotMenuUI, inputRouter, menuModule, sfxPlayer, systemSounds, saveDataService)
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

            PlaySound(m_SystemSounds.SaveGame);

            ShowSlots(file);
        }
    }
}
