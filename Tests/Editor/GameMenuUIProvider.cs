using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.Tests
{
    /// <summary>A UI provider of a game's own rather than the asset, holding a layout for each menu it is given.</summary>
    internal sealed class GameMenuUIProvider : IMenuUIProvider
    {
        private readonly Dictionary<ulong, VisualTreeAsset> m_Layouts;

        internal GameMenuUIProvider()
        {
            m_Layouts = new Dictionary<ulong, VisualTreeAsset>();
        }

        internal void Add(ulong menuId, VisualTreeAsset layout)
        {
            m_Layouts.Add(menuId, layout);
        }

        VisualTreeAsset IMenuUIProvider.GetLayout(ulong menuId) => m_Layouts[menuId];

        VisualTreeAsset IMenuUIProvider.SaveSlot => null;
    }
}
