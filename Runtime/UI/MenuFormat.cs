using System.Globalization;
using RPGFramework.Localisation;

namespace RPGFramework.Menu.UI
{
    internal static class MenuFormat
    {
        /// <summary>Hours and minutes.</summary>
        internal static string PlayTime(uint seconds) => $"{seconds / 3600}:{seconds / 60 % 60:00}";

        /// <summary>The culture of the language the game is shown in, for its dates and numbers.</summary>
        internal static CultureInfo CultureOf(ILocalisationService localisation) => CultureInfo.GetCultureInfo(localisation.CurrentLanguage);
    }
}
