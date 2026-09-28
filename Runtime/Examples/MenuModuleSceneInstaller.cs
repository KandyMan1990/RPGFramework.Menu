using RPGFramework.DI;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine;

namespace RPGFramework.Menu
{
    public class MenuModuleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindTransient<IBeginMenu, BeginMenu>();
            container.BindTransient<IBeginMenuUI, BeginMenuUI>();
            container.BindTransient<IConfigMenu, ConfigMenu>();
            container.BindTransient<IConfigMenuUI, ConfigMenuUI>();
            container.BindTransient<IPartyMenu, PartyMenu>();
            container.BindTransient<IPartyMenuUI, PartyMenuUI>();
            container.BindTransient<ISaveMenu, SaveMenu>();
            container.BindTransient<ILoadMenu, LoadMenu>();
            container.BindTransient<ISaveSlotMenuUI, SaveSlotMenuUI>();
        }
    }
}