using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using UnityEngine.UIElements;

namespace RPGFramework.Menu.UI
{
    /// <summary>Why a save cannot be loaded, by the localisation key for each reason; hidden for one that can.</summary>
    [UxmlElement]
    public sealed partial class SaveSlotStatus : Label, ISaveSlotElement
    {
        /// <summary>The key shown for a damaged save.</summary>
        [UxmlAttribute]
        public string DamagedKey { get; set; }

        /// <summary>The key shown for a save written by a newer version of the game.</summary>
        [UxmlAttribute]
        public string NewerVersionKey { get; set; }

        void ISaveSlotElement.Show(SavePreview preview, ILocalisationService localisation)
        {
            string key = preview.IsDamaged          ? DamagedKey
                       : preview.IsFromNewerVersion ? NewerVersionKey
                                                    : null;

            style.display = key == null ? DisplayStyle.None : DisplayStyle.Flex;
            text          = key == null ? string.Empty : localisation.Get(key);
        }
    }
}
