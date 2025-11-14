using System.Threading.Tasks;
using RPGFramework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGFramework.Menu
{
    public class MenuModule : IMenuModule
    {
        async Task IModule.OnEnterAsync(IModuleArgs args)
        {
            await SceneManager.LoadSceneAsync(nameof(MenuModule));
        }

        Task IModule.OnExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}