using System.Threading.Tasks;
using RPGFramework.Core;

namespace RPGFramework.Menu.BeginMenu
{
    public class BeginMenu : IBeginMenu
    {
        Task IMenu.OnEnterAsync()
        {
            return Task.CompletedTask;
        }

        Task IMenu.OnExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}