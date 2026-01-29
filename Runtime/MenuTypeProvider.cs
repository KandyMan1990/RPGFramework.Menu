using System;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu
{
    public class MenuTypeProvider : IMenuTypeProvider
    {
        private readonly IMenuTypeProvider m_MenuTypeProvider;

        public MenuTypeProvider()
        {
            m_MenuTypeProvider = this;
        }

        Type IMenuTypeProvider.GetType(MenuType type)
        {
            switch (type)
            {
                case MenuType.Inventory:
                    return null;
                case MenuType.Abilities:
                    return null;
                case MenuType.CharacterInfo:
                    return null;
                case MenuType.Config:
                    return typeof(IConfigMenu);
                case MenuType.Save:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(IMenuTypeProvider)}::{nameof(IMenuTypeProvider.GetType)} [{type}] not implemented");
            }
        }

        Type IMenuTypeProvider.GetType(byte type)
        {
            MenuType menuType = (MenuType)type;

            return m_MenuTypeProvider.GetType(menuType);
        }
    }
}