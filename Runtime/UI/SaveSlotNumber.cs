using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>A number a save holds, such as its money, written as the game's language writes numbers.</summary>
    [UxmlElement]
    public sealed partial class SaveSlotNumber : Label, ISaveSlotElement
    {
        /// <summary>The value shown, named as scripts name it: <c>Money</c>, <c>Party[0].Level</c>.</summary>
        [UxmlAttribute]
        public string Variable { get; set; }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            text = preview.ReadNumber(Variable).ToString("#,0.##", MenuFormat.CultureOf(localisation));
        }
    }
}
