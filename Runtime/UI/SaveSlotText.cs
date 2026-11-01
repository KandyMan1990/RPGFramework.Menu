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

        /// <summary>The sheet the key is in, such as <c>Locations</c>, which the save menu loads.</summary>
        [UxmlAttribute]
        public string Sheet
        {
            get => m_Text.Sheet;
            set => m_Text.Sheet = value;
        }

        private readonly LocalisedText m_Text;

        public SaveSlotText()
        {
            m_Text = new LocalisedText();

            SetBinding("text", m_Text);
        }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            m_Text.Hash = preview.Read<ulong>(Variable);
        }
    }
}
