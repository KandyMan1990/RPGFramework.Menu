using RPGFramework.Core;
using RPGFramework.DI;
using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SubMenus;
using RPGFramework.Menu.SubMenus.UI;
using UnityEngine;

namespace RPGFramework.Menu
{
    internal sealed class MenuModuleMonoBehaviour : MonoBehaviour, IModuleBindings
    {
        void IModuleBindings.Bind(IDIContainer sceneContainer)
        {
            sceneContainer.BindTransientIfNotRegistered<IBeginMenu, BeginMenu>();
            sceneContainer.BindTransientIfNotRegistered<IBeginMenuUI, BeginMenuUI>();
            sceneContainer.BindTransientIfNotRegistered<IConfigMenu, ConfigMenu>();
            sceneContainer.BindTransientIfNotRegistered<IConfigMenuUI, ConfigMenuUI>();
            sceneContainer.BindTransientIfNotRegistered<ILanguageMenu, LanguageMenu>();
            sceneContainer.BindTransientIfNotRegistered<ILanguageMenuUI, LanguageMenuUI>();
            sceneContainer.BindTransientIfNotRegistered<IPartyMenu, PartyMenu>();
            sceneContainer.BindTransientIfNotRegistered<IPartyMenuUI, PartyMenuUI>();
            sceneContainer.BindTransientIfNotRegistered<ISaveMenu, SaveMenu>();
            sceneContainer.BindTransientIfNotRegistered<ILoadMenu, LoadMenu>();
            sceneContainer.BindTransientIfNotRegistered<ISaveSlotMenuUI, SaveSlotMenuUI>();

            sceneContainer.BindSingletonIfNotRegistered<IMenuTypeProvider, MenuTypeProvider>();
            sceneContainer.BindSingletonIfNotRegistered<IMenuModule, MenuModule>();
        }
    }
}
