using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Menu.SharedTypes;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    /// <summary>The UXML layout each menu's UI is built from, set by the game.</summary>
    public interface IMenuUIProvider
    {
        /// <summary>The layout for the menu UI of this type, such as <see cref="IBeginMenuUI" />.</summary>
        VisualTreeAsset GetMenuUI<T>() where T : IMenuUI;
    }

    /// <summary>Which menu interface each <see cref="MenuType" /> names, for the menu module to resolve.</summary>
    public interface IMenuTypeProvider
    {
        /// <summary>The interface the menu of this type is bound to; null for a menu not built yet.</summary>
        Type GetType(MenuType type);

        /// <summary>The same, for a menu type stored as its number, as <see cref="MenuArgs" /> carries it.</summary>
        Type GetType(byte type);
    }

    /// <summary>
    /// One menu screen's behaviour, on the menu module's stack: entered, suspended under the next, resumed as that one
    /// closes, and exited. Its <see cref="IMenuUI" /> is what it shows.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Whether the menu under this one hides while this one is open, rather than showing beneath it.
        /// </summary>
        bool HidePreviousUIOnSuspend { get; }

        /// <summary>Builds the menu's UI in <paramref name="parent" /> and starts taking input.</summary>
        Task OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary

        /// <summary>Another menu opens over this one: it stops taking input, and hides its UI when asked to.</summary>
        Task OnSuspendAsync(bool hideUI);

        /// <summary>The menu over this one has closed: its UI shows again and it takes input.</summary>
        Task OnResumeAsync();

        /// <summary>The menu closes for good, its UI removed.</summary>
        Task OnExitAsync();
    }

    /// <summary>
    /// What a menu shows: built from the provider's layout, localised, and reporting the player's presses to its
    /// <see cref="IMenu" />.
    /// </summary>
    public interface IMenuUI
    {
        /// <summary>Raised when the player presses back in the UI.</summary>
        event Action OnBackButtonPressed;

        /// <summary>The element focused when the menu opens.</summary>
        VisualElement GetDefaultFocusedElement();

        /// <summary>The element focused when the menu was last suspended, for focus to return to.</summary>
        VisualElement GetLastFocusedElement();

        /// <summary>
        /// Builds the UI in <paramref name="parent" />, localises it, focuses its default element and shows it.
        /// </summary>
        Task OnEnterAsync(VisualElement parent, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary

        /// <summary>
        /// Remembers the focused element and stops reacting, hiding the UI when <paramref name="hideUI" />.
        /// </summary>
        Task OnSuspendAsync(bool hideUI);

        /// <summary>Shows the UI again, with focus back where it was.</summary>
        Task OnResumeAsync();

        /// <summary>Removes the UI.</summary>
        Task OnExitAsync();
    }

    /// <summary>
    /// The Language menu: choosing the language the game is shown in, on first launch and from Config.
    /// </summary>
    public interface ILanguageMenu : IMenu
    {
    }

    /// <summary>The title menu: New Game, Load Game and Quit.</summary>
    public interface IBeginMenu : IMenu
    {
    }

    /// <summary>The Config menu: language, volumes and message speeds, saved to the player's settings.</summary>
    public interface IConfigMenu : IMenu
    {
    }

    /// <summary>The party menu opened from the field: where the player is, the play time, Config and Save.</summary>
    public interface IPartyMenu : IMenu
    {
    }

    /// <summary>The Save menu: a new save or one written over, from the party menu where saving is allowed.</summary>
    public interface ISaveMenu : IMenu
    {
    }

    /// <summary>The Load menu: the saves to begin a playthrough from, from the title.</summary>
    public interface ILoadMenu : IMenu
    {
    }

    /// <summary>The Language menu's UI.</summary>
    public interface ILanguageMenuUI : IMenuUI
    {
        /// <summary>Raised with -1 or 1 as the player steps to the previous or next language.</summary>
        event Action<int> OnLanguageChanged;

        /// <summary>Shows every label again in the language now chosen.</summary>
        void RefreshLocalisation();
    }

    /// <summary>The title menu's UI.</summary>
    public interface IBeginMenuUI : IMenuUI
    {
        /// <summary>Raised when the player chooses New Game.</summary>
        event Action OnNewGamePressed;

        /// <summary>Raised when the player chooses Load Game.</summary>
        event Action OnLoadGamePressed;

        /// <summary>
        /// Raised when the player chooses Quit, with the task of its cancel sound playing out, for quitting to wait on.
        /// </summary>
        event Action<Task> OnQuitPressed;
    }

    /// <summary>The Config menu's UI.</summary>
    public interface IConfigMenuUI : IMenuUI
    {
        /// <summary>Raised with -1 or 1 as the player steps to the previous or next language.</summary>
        event Action<int> OnLanguageChanged;

        /// <summary>Raised when the player chooses Controls.</summary>
        event Action OnControlsPressed;

        /// <summary>Raised with the music volume, 0 to 1, as the player moves its slider.</summary>
        event Action<float> OnMusicVolumeChanged;

        /// <summary>Raised with the sound effects' volume, 0 to 1, as the player moves its slider.</summary>
        event Action<float> OnSfxVolumeChanged;

        /// <summary>Raised with the battle message speed, 0 to 1, as the player moves its slider.</summary>
        event Action<float> OnBattleMessageSpeedChanged;

        /// <summary>Raised with the field message speed, 0 to 1, as the player moves its slider.</summary>
        event Action<float> OnFieldMessageSpeedChanged;

        /// <summary>Shows every label again in the language now chosen.</summary>
        void RefreshLocalisation();

        /// <summary>Puts the music volume slider at a value, 0 to 1, without raising its event.</summary>
        void SetMusicVolume(float volume);

        /// <summary>Puts the sound effects' volume slider at a value, 0 to 1, without raising its event.</summary>
        void SetSfxVolume(float volume);

        /// <summary>Puts the battle message speed slider at a value, 0 to 1, without raising its event.</summary>
        void SetBattleMessageSpeed(float speed);

        /// <summary>Puts the field message speed slider at a value, 0 to 1, without raising its event.</summary>
        void SetFieldMessageSpeed(float speed);
    }

    /// <summary>The party menu's UI.</summary>
    public interface IPartyMenuUI : IMenuUI
    {
        /// <summary>Raised when the player chooses Config.</summary>
        event Action OnConfigPressed;

        /// <summary>Raised when the player chooses Save, allowed or not.</summary>
        event Action OnSavePressed;

        /// <summary>
        /// Greys Save out when saving is not allowed here; it stays focusable, so choosing it can say no.
        /// </summary>
        void SetSaveEnabled(bool enabled);

        /// <summary>Shows where the player is, by the place's localisation key hash.</summary>
        void SetLocationName(ulong keyHash);

        /// <summary>Shows the play time as hours and minutes.</summary>
        void SetPlayTime(uint seconds);
    }

    /// <summary>The save slots' UI, shared by the Save and Load menus.</summary>
    public interface ISaveSlotMenuUI : IMenuUI
    {
        /// <summary>Raised with a save's index in the list as the player chooses its slot.</summary>
        event Action<int> OnSlotChosen;

        /// <summary>Raised when the player chooses the new save row, which only saving shows.</summary>
        event Action OnNewSaveChosen;

        /// <summary>Raised with the player's answer to the question asked, true for yes.</summary>
        event Action<bool> OnQuestionAnswered;

        /// <summary>
        /// Whether the slots are for saving, with a new save row, rather than loading; set before the slots.
        /// </summary>
        void SetSaving(bool saving);

        /// <summary>
        /// Shows a row for each save, focusing the one at <paramref name="focusIndex" />, or the first row below 0.
        /// </summary>
        void SetSlots(IReadOnlyList<SaveSlotInfo> slots, int focusIndex);

        /// <summary>The focused save's index in the list, or -1 when no save's row is focused.</summary>
        int GetFocusedSlot();

        /// <summary>Asks whether to write over the focused save.</summary>
        void AskToOverwrite();

        /// <summary>Asks whether to delete the focused save.</summary>
        void AskToDelete();

        /// <summary>Closes the question, focus going back to the row it asked about.</summary>
        void CloseQuestion();
    }
}