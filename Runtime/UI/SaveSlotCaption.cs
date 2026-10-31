using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Text a save slot's layout names by its localisation key, such as what its money is called, from a sheet the save
    /// menu loads.
    /// </summary>
    [UxmlElement]
    public sealed partial class SaveSlotCaption : Label, ISaveSlotElement
    {
        /// <summary>The localisation key shown, such as <c>Generic/Money</c>.</summary>
        [UxmlAttribute]
        public string Key { get; set; }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            text = localisation.Get(Key);
        }
    }
}
