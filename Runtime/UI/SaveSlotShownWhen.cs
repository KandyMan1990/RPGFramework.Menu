using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>
    /// Holds what a save slot shows only for a save that cannot be loaded for one reason — damaged, say, holding the
    /// text saying so.
    /// </summary>
    [UxmlElement]
    public sealed partial class SaveSlotShownWhen : VisualElement, ISaveSlotElement
    {
        /// <summary>The reason it is shown for.</summary>
        [UxmlAttribute]
        public SaveSlotProblem Problem { get; set; }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            // A damaged save is only ever shown as damaged, as its version cannot be trusted.
            bool shown = Problem == SaveSlotProblem.Damaged
                ? preview.IsDamaged
                : !preview.IsDamaged && preview.IsFromNewerVersion;

            style.display = shown ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
