using RPGFramework.Core.Transitions;
using UnityEngine;

namespace RPGFramework.Menu
{
    /// <summary>How the screen changes from one menu to the next: the transitions covering and revealing it, or none.</summary>
    public interface IMenuTransitionConfig
    {
        /// <summary>Whether a menu change covers the screen at all; without, the menus swap at once.</summary>
        bool Covers { get; }

        /// <summary>The transition covering each menu change, as its name's hash; 0 for the game's default cover.</summary>
        ulong Cover { get; }

        /// <summary>The transition revealing each menu change, as its name's hash; 0 for the game's default reveal.</summary>
        ulong Reveal { get; }
    }

    [CreateAssetMenu(menuName = "RPG Framework/Menu/Menu Transition Config", fileName = "MenuTransitionConfig")]
    public sealed class MenuTransitionConfig : ScriptableObject, IMenuTransitionConfig
    {
        [SerializeField]
        [Tooltip("Off, the menus swap at once, nothing covering the change")]
        private bool m_Covers = true;

        [SerializeField]
        [Tooltip("Covers each menu change; empty for the game's default cover")]
        private TransitionAsset m_Cover;

        [SerializeField]
        [Tooltip("Reveals each menu change; empty for the game's default reveal")]
        private TransitionAsset m_Reveal;

        bool IMenuTransitionConfig. Covers => m_Covers;
        ulong IMenuTransitionConfig.Cover  => NameHashOf(m_Cover);
        ulong IMenuTransitionConfig.Reveal => NameHashOf(m_Reveal);

        private static ulong NameHashOf(TransitionAsset transition)
        {
            ulong nameHash = transition != null ? transition.NameHash : 0;

            return nameHash;
        }
    }
}