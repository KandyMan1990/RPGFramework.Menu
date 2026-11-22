using System.Collections.Generic;
using RPGFramework.Core.Databases;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// One slot of the party, holding the <see cref="IPartyMemberElement" />s that show its member: in a save slot, from
    /// the save; in a menu, from the playthrough. Hidden while the slot is empty, keeping its place in the layout.
    /// </summary>
    [UxmlElement]
    public sealed partial class PartyMember : VisualElement
    {
        /// <summary>The party slot shown, 0 the leader.</summary>
        // Not "slot", which UXML keeps for a template's slots and never passes to the element.
        [UxmlAttribute]
        public int PartySlot { get; set; }

        private IPartyMemberElement[] m_Elements;

        public PartyMember()
        {
            m_Elements = null;
        }

        internal void Show(SavePreview values, ICharacterDatabase characters, ILocalisationService localisation)
        {
            byte stored = values.Read<byte>($"{CoreVariables.PARTY}[{PartySlot}]");
            int  record = stored == CoreVariables.EMPTY_PARTY_SLOT ? -1 : stored;

            CharacterDefinition character = null;

            if (record >= 0)
            {
                characters.TryGetDefinition(values.Read<ulong>($"{CoreVariables.ROSTER}[{record}].{CoreVariables.ROSTER_CHARACTER}"), out character);
            }

            style.visibility = record < 0 ? Visibility.Hidden : Visibility.Visible;

            m_Elements ??= FindElements();

            for (int i = 0; i < m_Elements.Length; i++)
            {
                m_Elements[i].Show(values, record, character, localisation);
            }
        }

        // The layout is fixed once made, so its elements are found once; counting them first would take the same walk.
        private IPartyMemberElement[] FindElements()
        {
            List<VisualElement>       descendants = this.Query<VisualElement>().ToList();
            List<IPartyMemberElement> elements    = new List<IPartyMemberElement>();

            for (int i = 0; i < descendants.Count; i++)
            {
                if (descendants[i] is IPartyMemberElement element)
                {
                    elements.Add(element);
                }
            }

            return elements.ToArray();
        }
    }
}
