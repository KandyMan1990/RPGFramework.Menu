using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    public interface IMenuUIProvider
    {
        VisualTreeAsset GetMenuUI<T>() where T : IMenu;
    }

    public interface IMenu
    {
        Task OnEnterAsync(VisualElement rootContainer);
        Task OnSuspendAsync();
        Task OnResumeAsync();
        Task OnExitAsync();
    }

    public interface IMenuUI
    {
        event Action<int> OnPlayAudio;
        Task              OnEnterAsync(VisualElement rootContainer);
        Task              OnSuspendAsync();
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