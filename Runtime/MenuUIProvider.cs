using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    [CreateAssetMenu(fileName = "Menu UI Provider", menuName = "RPG Framework/Menu/Menu UI Provider")]
    public sealed class MenuUIProvider : ScriptableObject, IMenuUIProvider
    {
        [SerializeField]
        private VisualTreeAsset m_BeginMenu;
        [SerializeField]
        private VisualTreeAsset m_ConfigMenu;
        [SerializeField]
        private VisualTreeAsset m_LanguageMenu;
        [SerializeField]
        private VisualTreeAsset m_ControlsMenu;
        [SerializeField]
        private VisualTreeAsset m_PartyMenu;
        [SerializeField]
        private VisualTreeAsset m_SaveMenu;
        [SerializeField]
        private VisualTreeAsset m_SaveSlot;

        VisualTreeAsset IMenuUIProvider.SaveSlot => m_SaveSlot;

        VisualTreeAsset IMenuUIProvider.GetMenuUI<T>()
        {
            Type type = typeof(T);

            if (type == typeof(IBeginMenuUI)) return m_BeginMenu;
            if (type == typeof(IConfigMenuUI)) return m_ConfigMenu;
            if (type == typeof(ILanguageMenuUI)) return m_LanguageMenu;
            if (type == typeof(IControlsMenuUI)) return m_ControlsMenu;
            if (type == typeof(IPartyMenuUI)) return m_PartyMenu;
            if (type == typeof(ISaveSlotMenuUI)) return m_SaveMenu;

            throw new NotImplementedException($"The type {type} is not implemented");
        }
    }
}