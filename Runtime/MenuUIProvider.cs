using System;
using System.Collections.Generic;
using RPGFramework.Hashing;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    [CreateAssetMenu(fileName = "Menu UI Provider", menuName = "RPG Framework/Menu/Menu UI Provider")]
    public sealed class MenuUIProvider : ScriptableObject, IMenuUIProvider
    {
        [SerializeField]
        private MenuLayout[] m_Layouts = Array.Empty<MenuLayout>();

        [SerializeField]
        private VisualTreeAsset m_SaveSlot;

        private Dictionary<ulong, VisualTreeAsset> m_ByMenu;

        VisualTreeAsset IMenuUIProvider.SaveSlot => m_SaveSlot;

#if UNITY_EDITOR
        // As it is edited; a player only ever loads it.
        private void OnValidate()
        {
            BuildLookup();
        }
#endif

        private void OnEnable()
        {
            BuildLookup();
        }

        VisualTreeAsset IMenuUIProvider.GetLayout(ulong menuId)
        {
            return m_ByMenu[menuId];
        }

        private void BuildLookup()
        {
            m_ByMenu = new Dictionary<ulong, VisualTreeAsset>(m_Layouts.Length);

            for (int i = 0; i < m_Layouts.Length; i++)
            {
                m_ByMenu[Fnv1a64.Hash(m_Layouts[i].Menu)] = m_Layouts[i].Layout;
            }
        }

        /// <summary>One menu's layout, under the menu's name.</summary>
        [Serializable]
        private struct MenuLayout
        {
            [SerializeField]
            [Tooltip("The menu's name: Begin, Config, Language, Controls, Party, Save or Load for the framework's, or a game's own")]
            private string m_Menu;

            [SerializeField]
            private VisualTreeAsset m_Layout;

            internal string          Menu   => m_Menu ?? string.Empty;
            internal VisualTreeAsset Layout => m_Layout;
        }
    }
}
