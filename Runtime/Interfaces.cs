using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Menu.SharedTypes;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    public interface IMenuUIProvider
    {
        VisualTreeAsset GetMenuUI<T>() where T : IMenuUI;
    }

    public interface IMenuTypeProvider
    {
        Type GetType(MenuType type);
        Type GetType(byte     type);
    }

    public interface IMenu
    {
        bool HidePreviousUiOnSuspend { get; }
        Task OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary
        Task OnSuspendAsync(bool        hideUi);
        Task OnResumeAsync();
        Task OnExitAsync();
    }

    public interface IMenuUI
    {
        event Action  OnBackButtonPressed;
        VisualElement GetDefaultFocusedElement();
        VisualElement GetLastFocusedElement();
        Task          OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary
        Task          OnSuspendAsync(bool        hideUi);
        Task          OnResumeAsync();
        Task          OnExitAsync();
    }

    public interface ILanguageMenu : IMenu
    {
    }

    public interface IBeginMenu : IMenu
    {
    }

    public interface IConfigMenu : IMenu
    {
    }

    public interface IPartyMenu : IMenu
    {
    }

    public interface ISaveMenu : IMenu
    {
    }

    public interface ILoadMenu : IMenu
    {
    }

    public interface ILanguageMenuUI : IMenuUI
    {
        event Action<int> OnLanguageChanged;
        void              RefreshLocalisation();
    }

    public interface IBeginMenuUI : IMenuUI
    {
        event Action OnNewGamePressed;
        event Action OnLoadGamePressed;
        event Action OnQuitPressed;
    }

    public interface IConfigMenuUI : IMenuUI
    {
        event Action<int>   OnLanguageChanged;
        event Action        OnControlsPressed;
        event Action<float> OnMusicVolumeChanged;
        event Action<float> OnSfxVolumeChanged;
        event Action<float> OnBattleMessageSpeedChanged;
        event Action<float> OnFieldMessageSpeedChanged;
        void                RefreshLocalisation();
        void                SetMusicVolume(float        volume);
        void                SetSfxVolume(float          volume);
        void                SetBattleMessageSpeed(float speed);
        void                SetFieldMessageSpeed(float  speed);
    }

    public interface IPartyMenuUI : IMenuUI
    {
        event Action OnConfigPressed;
        event Action OnSavePressed;
        void         SetSaveEnabled(bool   enabled);
        void         SetLocationName(ulong keyHash);
        void         SetPlayTime(uint      seconds);
    }

    public interface ISaveSlotMenuUI : IMenuUI
    {
        event Action<int>  OnSlotChosen;
        event Action       OnNewSaveChosen;
        event Action<bool> OnQuestionAnswered;
        void               SetSaving(bool                        saving);
        void               SetSlots(IReadOnlyList<SaveSlotInfo> slots, int focusIndex);
        int                GetFocusedSlot();
        void               AskToOverwrite();
        void               AskToDelete();
        void               CloseQuestion();
    }
}