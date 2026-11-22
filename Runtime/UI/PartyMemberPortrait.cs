using RPGFramework.Core.Databases;
using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>The member's portrait, as their definition gives it; none for an empty slot.</summary>
    [UxmlElement]
    public sealed partial class PartyMemberPortrait : Image, IPartyMemberElement
    {
        void IPartyMemberElement.Show(SavePreview values, int record, CharacterDefinition character, ILocalisationService localisation)
        {
            sprite = character?.Portrait;
        }
    }
}
