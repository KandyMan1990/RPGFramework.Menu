using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>When a save was written, as the game's language writes a date and time.</summary>
    [UxmlElement]
    public sealed partial class SaveSlotDate : Label, ISaveSlotElement
    {
        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            text = preview.LastWritten.ToString("g", MenuFormat.CultureOf(localisation));
        }
    }
}
