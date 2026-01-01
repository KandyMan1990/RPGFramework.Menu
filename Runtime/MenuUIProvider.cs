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

        VisualTreeAsset IMenuUIProvider.GetMenuUI<T>()
        {
            Type type = typeof(T);

            if (type == typeof(IBeginMenuUI)) return m_BeginMenu;
            if (type == typeof(IConfigMenuUI)) return m_ConfigMenu;

            throw new NotImplementedException($"The type {type} is not implemented");
        }
    }
}