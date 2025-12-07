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
        Task              OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary
        Task              OnSuspendAsync(bool        hideUi);
        Task              OnResumeAsync();
        Task              OnExitAsync();
    }

    public interface IBeginMenu : IMenu
    {

    }

    public interface IBeginMenuUI : IMenuUI
    {
        event Action OnNewGame;
        event Action OnLoadGame;
        event Action OnSettings;
        event Action OnQuit;
    }
}