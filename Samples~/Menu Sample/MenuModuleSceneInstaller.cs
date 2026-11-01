using RPGFramework.DI;
using UnityEngine;

namespace RPGFramework.Menu.Menu_Sample
{
    /// <summary>
    /// The menus' text names its keys in their layouts, which the UI provider holds, so the menus load the sheets those
    /// keys are in: the sheets this sample ships.
    /// </summary>
    public sealed class MenuModuleSceneInstaller : SceneInstallerBase
    {
        [SerializeField]
        private MenuUIProvider m_MenuUIProvider;

        [SerializeField]
        private MenuTransitionConfig m_TransitionConfig;

        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
            container.BindSingletonFromInstance<IMenuTransitionConfig>(m_TransitionConfig);
        }
    }
}
