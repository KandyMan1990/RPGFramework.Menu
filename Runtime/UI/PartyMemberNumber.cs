using RPGFramework.Core.Databases;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// A number the member's record holds, a field of the game's such as their level, written as the game's language
    /// writes numbers; empty for an empty slot.
    /// </summary>
    [UxmlElement]
    public sealed partial class PartyMemberNumber : Label, IPartyMemberElement
    {
        /// <summary>The field of the member's <c>Roster</c> record shown, as scripts name it: <c>Level</c>, <c>Stats[2]</c>.</summary>
        [UxmlAttribute]
        public string Field { get; set; }

        void IPartyMemberElement.Show(SavePreview values, int record, CharacterDefinition character, ILocalisationService localisation)
        {
            text = record < 0
                       ? string.Empty
                       : values.ReadNumber($"{CoreVariables.ROSTER}[{record}].{Field}").ToString("#,0.##", MenuFormat.CultureOf(localisation));
        }
    }
}
