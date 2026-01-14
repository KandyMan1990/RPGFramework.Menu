using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    [CreateAssetMenu(fileName = "Menu UI Provider", menuName = "RPG Framework/Menu/Menu UI Provider")]
    public class MenuUIProvider : ScriptableObject, IMenuUIProvider
    {
        [SerializeField]
        private VisualTreeAsset m_BeginMenu;
        [SerializeField]
        private VisualTreeAsset m_ConfigMenu;
        [SerializeField]
        private VisualTreeAsset m_LanguageMenu;

        VisualTreeAsset IMenuUIProvider.GetMenuUI<T>()
        {
            Type type = typeof(T);

            if (type == typeof(IBeginMenuUI)) return m_BeginMenu;
            if (type == typeof(IConfigMenuUI)) return m_ConfigMenu;
            if (type == typeof(ILanguageMenuUI)) return m_LanguageMenu;

            throw new NotImplementedException($"The type {type} is not implemented");
        }
    }
}