using RPGFramework.Core;
using RPGFramework.Core.Audio;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu.SubMenus
{
    public class LoadMenu : SaveSlotMenu, ILoadMenu
    {
        protected override bool IsSaving => false;

        private readonly ICurrentModuleStore m_CurrentModuleStore;
        private readonly IChangeModuleStore  m_ChangeModuleStore;
        private readonly IPlayTimeStore      m_PlayTimeStore;

        public LoadMenu(ISaveSlotMenuUI     saveSlotMenuUI,
                        IInputRouter        inputRouter,
                        IMenuModule         menuModule,
                        IAudioIntentPlayer  audioIntentPlayer,
                        ISaveDataService    saveDataService,
                        ICurrentModuleStore currentModuleStore,
                        IChangeModuleStore  changeModuleStore,
                        IPlayTimeStore      playTimeStore) : base(saveSlotMenuUI, inputRouter, menuModule, audioIntentPlayer, saveDataService)
        {
            m_CurrentModuleStore = currentModuleStore;
            m_ChangeModuleStore  = changeModuleStore;
            m_PlayTimeStore      = playTimeStore;
        }

        protected override void OnSlotChosen(int index)
        {
            if (!CanLoad(index))
            {
                m_AudioIntentPlayer.Play(AudioIntent.Error, AudioContext.Menu);
                return;
            }

            m_SaveDataService.BeginSave(m_Files[index]);

            m_ChangeModuleStore.SetModuleId(m_CurrentModuleStore.GetModuleId);

            m_AudioIntentPlayer.Play(AudioIntent.LoadGame, AudioContext.Menu);

            m_PlayTimeStore.StartCounting();

            m_MenuModule.RequestModuleChange();
            m_MenuModule.PopMenu().FireAndForget();
        }
    }
}