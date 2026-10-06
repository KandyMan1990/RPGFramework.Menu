using RPGFramework.Menu.SharedTypes;
using RPGFramework.Menu.SharedTypes.Stores;

namespace RPGFramework.Menu
{
    public sealed class MenuArgsStore : IMenuArgsStore
    {
        private MenuArgs m_Args;

        MenuArgs IMenuArgsStore.Args => m_Args;

        void IMenuArgsStore.Set(MenuArgs args) => m_Args = args;
    }
}
