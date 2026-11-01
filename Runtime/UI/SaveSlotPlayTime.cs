using RPGFramework.Core.Memory;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.UI;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>A save's play time, as hours and minutes.</summary>
    [UxmlElement]
    public sealed partial class SaveSlotPlayTime : Label, ISaveSlotElement
    {
        /// <summary>The seconds shown, named as scripts name them; Core's play time unless set.</summary>
        [UxmlAttribute]
        public string Variable { get; set; }

        public SaveSlotPlayTime()
        {
            Variable = CoreVariables.PLAY_TIME;
        }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            text = ClockText.PlayTime((uint)preview.ReadNumber(Variable), true);
        }
    }
}
