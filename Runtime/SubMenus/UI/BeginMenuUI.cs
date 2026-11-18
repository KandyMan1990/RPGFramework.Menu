using RPGFramework.Localisation;
using RPGFramework.Menu.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.SubMenus.UI
{
    internal sealed class BeginMenuUI : MenuUI<IBeginMenuUI>, IBeginMenuUI
    {
        private Label m_VersionLabel;

        public BeginMenuUI(IMenuUIProvider uiProvider, ILocalisationService localisationService)
            : base(uiProvider, localisationService)
        {
        }

#if ENABLE_RPG_FRAMEWORK_DEBUG
        // Added in code, so a game's layout holds nothing for it: before Quit's entry, or after the last without one.
        protected override void AddEntries()
        {
            MenuEntry quit = m_UIInstance.Query<MenuEntry>().Where(entry => entry.Command == BeginMenuCommands.QUIT).First();
            MenuEntry last = m_UIInstance.Query<MenuEntry>().Last();
            MenuEntry room = new MenuEntry { name = "DebugRoomBtn", text = "Debug Room", Command = BeginMenuCommands.DEBUG_ROOM };

            if (quit != null)
            {
                quit.parent.Insert(quit.parent.IndexOf(quit), room);
            }
            else if (last != null)
            {
                last.parent.Insert(last.parent.IndexOf(last) + 1, room);
            }
        }
#endif

        protected override void HookupUI()
        {
            m_VersionLabel = m_UIInstance.Q<Label>("VersionLabel");

            m_VersionLabel.text = $"v{Application.version}";
        }
    }
}
