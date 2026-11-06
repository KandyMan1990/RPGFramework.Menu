using RPGFramework.Audio;
using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    internal sealed class LoadMenu : SaveSlotMenu, ILoadMenu
    {
        protected override bool IsSaving => false;

        private readonly IPlayTimeStore m_PlayTimeStore;

        public LoadMenu(ISaveSlotMenuUI     saveSlotMenuUI,
                        IInputRouter        inputRouter,
                        IMenuModule         menuModule,
                        ISfxPlayer          sfxPlayer,
                        ISystemSoundConfig  systemSounds,
                        ISaveDataService    saveDataService,
                        IPlayTimeStore      playTimeStore) : base(saveSlotMenuUI, inputRouter, menuModule, sfxPlayer, systemSounds, saveDataService)
        {
            m_PlayTimeStore = playTimeStore;
        }

        protected override void OnSlotChosen(int index)
        {
            if (!CanLoad(index))
            {
                PlaySound(m_SystemSounds.Error);
                return;
            }

            m_SaveDataService.BeginSave(m_Files[index]);

            PlaySound(m_SystemSounds.LoadGame);

            m_PlayTimeStore.StartCounting();

            m_MenuModule.RequestModuleChange(MenuOutcomes.PLAYTHROUGH);
            m_MenuModule.PopMenu().FireAndForget();
        }
    }
}