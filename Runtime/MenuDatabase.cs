using System;
using System.Collections.Generic;
using RPGFramework.Menu.SharedTypes.Constants;

namespace RPGFramework.Menu
{
    /// <summary>
    /// The framework's menus, and any a game adds. The menu scene binds one of its own unless the game's installer has;
    /// a game with menus of its own makes one there, adds them as an <see cref="IMenuDatabaseBuilder" />, and binds it as
    /// <see cref="IMenuDatabase" />, beside the bindings of the menus' own classes.
    /// </summary>
    public sealed class MenuDatabase : IMenuDatabase, IMenuDatabaseBuilder
    {
        private readonly Dictionary<ulong, Type> m_Menus;

#if UNITY_EDITOR
        /// <summary>Every menu it holds, for the editor's check as the menu module enters.</summary>
        internal IReadOnlyDictionary<ulong, Type> Menus => m_Menus;
#endif

        public MenuDatabase()
        {
            m_Menus = new Dictionary<ulong, Type>
                      {
                          { MenuIds.Begin, typeof(IBeginMenu) },
                          { MenuIds.Config, typeof(IConfigMenu) },
                          { MenuIds.Language, typeof(ILanguageMenu) },
                          { MenuIds.Controls, typeof(IControlsMenu) },
                          { MenuIds.Party, typeof(IPartyMenu) },
                          { MenuIds.Save, typeof(ISaveMenu) },
                          { MenuIds.Load, typeof(ILoadMenu) }
                      };
        }

        void IMenuDatabaseBuilder.Add<TMenu>(ulong menuId)
        {
            m_Menus.Add(menuId, typeof(TMenu));
        }

        Type IMenuDatabase.GetMenuType(ulong menuId)
        {
            return m_Menus[menuId];
        }
    }
}
