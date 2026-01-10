using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    public interface IMenuUIProvider
    {
        VisualTreeAsset GetMenuUI<T>() where T : IMenuUI;
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
        event Action<int> OnPlayAudio;
        event Action      OnBackButtonPressed;
        VisualElement     GetDefaultFocusedElement();
        VisualElement     GetLastFocusedElement();
        Task              OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary
        Task              OnSuspendAsync(bool        hideUi);
        Task              OnResumeAsync();
        Task              OnExitAsync();
    }

    public interface IBeginMenu : IMenu
    {
    }

    public interface IConfigMenu : IMenu
    {
    }

    public interface IBeginMenuUI : IMenuUI
    {
        event Action OnNewGamePressed;
        event Action OnLoadGamePressed;
        event Action OnSettingsPressed;
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
}