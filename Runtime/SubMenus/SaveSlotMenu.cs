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
        protected override bool m_HidePreviousUiOnSuspend => true;

        protected readonly ISaveDataService m_SaveDataService;
        protected readonly List<string>     m_Files = new List<string>();

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
            m_MenuUI.OnSlotChosen        += OnSlotChosen;
            m_MenuUI.OnNewSaveChosen     += OnNewSaveChosen;
            m_MenuUI.OnOverwriteAnswered += OnOverwriteAnswered;
        }

        protected override void UnregisterCallbacks()
        {
            m_MenuUI.OnOverwriteAnswered -= OnOverwriteAnswered;
            m_MenuUI.OnNewSaveChosen     -= OnNewSaveChosen;
            m_MenuUI.OnSlotChosen        -= OnSlotChosen;
        }

        protected override bool HandleControl(ControlSlot slot)
        {
            if (slot == ControlSlot.Secondary)
            {
                OnBack();
            }

            return true;
        }

        protected virtual void OnBack()
        {
            m_AudioIntentPlayer.Play(AudioIntent.Cancel, AudioContext.Menu);
            m_MenuModule.PopMenu().FireAndForget();
        }

        protected abstract void OnSlotChosen(int index);

        protected virtual void OnNewSaveChosen()
        {
        }

        protected virtual void OnOverwriteAnswered(bool overwrite)
        {
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

            foreach (SavePreview preview in previews)
            {
                if (preview.FileName == focusFile)
                {
                    focusIndex = m_Files.Count;
                }

                m_Files.Add(preview.FileName);
                slots.Add(new SaveSlotInfo(preview.Read<ulong>(CoreVariables.LOCATION_NAME), preview.Read<uint>(CoreVariables.PLAY_TIME), preview.LastWritten));
            }

            m_MenuUI.SetSlots(slots, focusIndex);
        }
    }
}