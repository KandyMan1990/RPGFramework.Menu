using System;
using RPGFramework.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    [CreateAssetMenu(fileName = "Menu UI Provider", menuName = "RPG Framework/Menu/Menu UI Provider")]
    public class MenuUIProvider : ScriptableObject, IMenuUIProvider
    {
        [SerializeField]
        private VisualTreeAsset m_BeginMenu;

        VisualTreeAsset IMenuUIProvider.GetMenuUI<T>()
        {
            Type type = typeof(T);

            if (type == typeof(IBeginMenu)) return m_BeginMenu;

            throw new NotImplementedException($"The type {type} is not implemented");
        }
    }
}