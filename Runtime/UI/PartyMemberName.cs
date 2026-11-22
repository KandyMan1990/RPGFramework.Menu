using RPGFramework.Core.Databases;
using RPGFramework.Core.SaveData;
using RPGFramework.Hashing;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>The member's name, as their definition's key gives it in the player's language; empty for an empty slot.</summary>
    [UxmlElement]
    public sealed partial class PartyMemberName : Label, IPartyMemberElement
    {
        /// <summary>The sheet the characters' names are in, such as <c>Characters</c>, which the menu loads.</summary>
        [UxmlAttribute, LocalisationSheet]
        public string Sheet
        {
            get => m_Text.Sheet;
            set => m_Text.Sheet = value;
        }

        private readonly DynamicLocalisedText m_Text;

        public PartyMemberName()
        {
            m_Text = new DynamicLocalisedText();

            SetBinding("text", m_Text);
        }

        void IPartyMemberElement.Show(SavePreview values, int record, CharacterDefinition character, ILocalisationService localisation)
        {
            m_Text.Hash = character == null || string.IsNullOrEmpty(character.NameKey) ? 0UL : Fnv1a64.Hash(character.NameKey);
        }
    }
}
