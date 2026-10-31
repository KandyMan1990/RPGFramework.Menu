using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>A text a save names by its localisation key's hash, such as where it was made; empty for none.</summary>
    [UxmlElement]
    public sealed partial class SaveSlotText : Label, ISaveSlotElement
    {
        /// <summary>The <c>ULong</c> holding the key's hash, named as scripts name it: <c>LocationName</c>.</summary>
        [UxmlAttribute]
        public string Variable { get; set; }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            text = localisation.TryGet(preview.Read<ulong>(Variable), out string value) ? value : string.Empty;
        }
    }
}
