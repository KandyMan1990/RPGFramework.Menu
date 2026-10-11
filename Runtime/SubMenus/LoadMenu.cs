using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu.SubMenus
{
    public class LoadMenu : SaveSlotMenu, ILoadMenu
    {
        protected override bool IsSaving => false;

        private readonly IPlayTimeStore m_PlayTimeStore;

        public LoadMenu(ISaveSlotMenuUI     saveSlotMenuUI,
                        IInputRouter        inputRouter,
                        IMenuModule         menuModule,
                        IAudioIntentPlayer  audioIntentPlayer,
                        ISaveDataService    saveDataService,
                        IPlayTimeStore      playTimeStore) : base(saveSlotMenuUI, inputRouter, menuModule, audioIntentPlayer, saveDataService)
        {
            m_PlayTimeStore = playTimeStore;
        }

        protected override void OnSlotChosen(int index)
        {
            if (!CanLoad(index))
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_SaveDataService.BeginSave(m_Files[index]);

            m_AudioIntentPlayer.Play(AudioIntent.LoadGame, AudioContext.Menu);

            m_PlayTimeStore.StartCounting();

            m_MenuModule.RequestModuleChange(MenuOutcomes.PLAYTHROUGH);
            m_MenuModule.PopMenu().FireAndForget();
        }
    }
}