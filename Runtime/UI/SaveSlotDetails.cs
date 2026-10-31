using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Holds what a save slot shows of the save's values, hidden for a damaged save, whose values read only their
    /// defaults.
    /// </summary>
    [UxmlElement]
    public sealed partial class SaveSlotDetails : VisualElement, ISaveSlotElement
    {
        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            style.display = preview.IsDamaged ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
