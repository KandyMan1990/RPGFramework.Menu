using System;
using RPGFramework.Menu.SharedTypes;

namespace RPGFramework.Menu
{
    public class MenuTypeProvider : IMenuTypeProvider
    {
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
    }
}