using RPGFramework.Core;
using RPGFramework.Core.DI;
using RPGFramework.Menu.SubMenus;
using UnityEngine;

namespace RPGFramework.Menu
{
    public class MenuModuleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        public override void InstallBindings(DIContainer container)
        {
            container.BindSingletonFromInstance<IMenuUIProvider, MenuUIProvider>(m_MenuUIProvider);
            container.BindTransient<IBeginMenu, BeginMenu>();
        }
    }
}