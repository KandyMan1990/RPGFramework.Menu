using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Core.Input;
using RPGFramework.Core.SaveData;
using RPGFramework.Localisation;
using RPGFramework.Menu.SharedTypes.Constants;
using UnityEngine.UIElements;

namespace RPGFramework.Menu
{
    /// <summary>The UXML layout each menu's UI is built from, set by the game.</summary>
    public interface IMenuUIProvider
    {
        /// <summary>
        /// The layout of the menu named by <paramref name="menuId" />, the hash of its name: one of the framework's
        /// <c>MenuIds</c>, or a game's own.
        /// </summary>
        VisualTreeAsset GetLayout(ulong menuId);

        /// <summary>
        /// The layout of one slot in the Save and Load menus, each of its <see cref="ISaveSlotElement" />s showing
        /// something of the save.
        /// </summary>
        VisualTreeAsset SaveSlot { get; }
    }

    /// <summary>
    /// An element of a save slot's layout that shows something of the save, filled as the slot is shown and again as
    /// the language changes. The framework's show a variable as a number, a play time or a text, a caption, the date
    /// and why a save cannot be loaded; a game's own, such as the party's portraits, implements this.
    /// </summary>
    public interface ISaveSlotElement
    {
        /// <summary>Shows <paramref name="preview" />, its text in <paramref name="localisation" />'s language.</summary>
        void Show(SavePreview preview, ILocalisationService localisation);
    }

    /// <summary>
    /// Each menu's id, the hash of its name, to the interface the menu is bound to, for the menu module to resolve:
    /// the framework's menus, and a game's own added through <see cref="IMenuDatabaseBuilder" /> in its menu scene's
    /// installer. Fixed once bound: what reads it cannot add to it.
    /// </summary>
    public interface IMenuDatabase
    {
        /// <summary>The interface the menu with this id is bound to, such as <see cref="IPartyMenu" />.</summary>
        Type GetMenuType(ulong menuId);
    }

    /// <summary>
    /// A <see cref="MenuDatabase" /> as a game's menu scene installer adds its own menus to it, before binding it as
    /// <see cref="IMenuDatabase" />.
    /// </summary>
    public interface IMenuDatabaseBuilder
    {
        /// <summary>
        /// Adds a game's own menu, opened as <paramref name="menuId" />, the hash of its name, and resolved as
        /// <typeparamref name="TMenu" />, the interface the game binds it to. A framework menu is replaced by binding its
        /// interface to the game's own class, not here.
        /// </summary>
        void Add<TMenu>(ulong menuId) where TMenu : IMenu;
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

        /// <summary>
        /// Builds the menu's UI in <paramref name="parent" />, from the layout the UI provider holds for
        /// <paramref name="menuId" />, without taking input yet.
        /// </summary>
        Task OnEnterAsync(VisualElement parent, ulong menuId, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary

        /// <summary>Another menu opens over this one: it hides its UI when asked to.</summary>
        Task OnSuspendAsync(bool hideUI);

        /// <summary>The menu over this one has closed: its UI shows again.</summary>
        Task OnResumeAsync();

        /// <summary>The menu closes for good, its UI removed.</summary>
        Task OnExitAsync();

        /// <summary>
        /// Starts or stops the menu and its UI reacting to the player. The menu module turns it on once a change has
        /// faded in and off as the next begins, so nothing pressed while the screen fades does anything.
        /// </summary>
        void SetInputEnabled(bool enabled);
    }

    /// <summary>
    /// What a menu shows: built from the provider's layout, localised, and reporting the player's presses to its
    /// <see cref="IMenu" />.
    /// </summary>
    public interface IMenuUI
    {
        /// <summary>Raised when the player presses back in the UI.</summary>
        event Action OnBackButtonPressed;

        /// <summary>Raised when the player moves the cursor to another button, for the menu to sound.</summary>
        event Action OnCursorMoved;

        /// <summary>The element focused when the menu opens.</summary>
        VisualElement GetDefaultFocusedElement();

        /// <summary>The element focused when the menu was last suspended, for focus to return to.</summary>
        VisualElement GetLastFocusedElement();

        /// <summary>
        /// Builds the UI in <paramref name="parent" /> from the layout the UI provider holds for
        /// <paramref name="menuId" />, localises it, focuses its default element and shows it.
        /// </summary>
        Task OnEnterAsync(VisualElement parent, ulong menuId, Dictionary<string, object> args = null); //TODO: args should probably be more strongly typed than a dictionary

        /// <summary>Remembers the focused element, hiding the UI when <paramref name="hideUI" />.</summary>
        Task OnSuspendAsync(bool hideUI);

        /// <summary>Shows the UI again, with focus back where it was.</summary>
        Task OnResumeAsync();

        /// <summary>Removes the UI.</summary>
        Task OnExitAsync();

        /// <summary>Starts or stops the UI reacting to presses, as its menu says.</summary>
        void SetInputEnabled(bool enabled);
    }

    /// <summary>
    /// The Language menu: choosing the language the game is shown in, on first launch and from Config.
    /// </summary>
    [MenuName(MenuIds.LANGUAGE)]
    public interface ILanguageMenu : IMenu
    {
    }

    /// <summary>The title menu: New Game, Load Game and Quit.</summary>
    [MenuName(MenuIds.BEGIN)]
    public interface IBeginMenu : IMenu
    {
    }

    /// <summary>The Config menu: language, volumes and message speeds, saved to the player's settings.</summary>
    [MenuName(MenuIds.CONFIG)]
    public interface IConfigMenu : IMenu
    {
    }

    /// <summary>The party menu opened from the field: where the player is, the play time, Config and Save.</summary>
    [MenuName(MenuIds.PARTY)]
    public interface IPartyMenu : IMenu
    {
    }

    /// <summary>The Save menu: a new save or one written over, from the party menu where saving is allowed.</summary>
    [MenuName(MenuIds.SAVE)]
    public interface ISaveMenu : IMenu
    {
    }

    /// <summary>The Load menu: the saves to begin a playthrough from, from the title.</summary>
    [MenuName(MenuIds.LOAD)]
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
        /// Raised when the player chooses Config, to set their preferences before playing. A title that offers no Config
        /// never raises it.
        /// </summary>
        event Action OnConfigPressed;

        /// <summary>Raised when the player chooses Quit.</summary>
        event Action OnQuitPressed;
    }

    /// <summary>The Config menu's UI.</summary>
    public interface IConfigMenuUI : IMenuUI
    {
        /// <summary>Raised with -1 or 1 as the player steps to the previous or next language.</summary>
        event Action<int> OnLanguageChanged;

        /// <summary>Raised when the player chooses Controls.</summary>
        event Action OnControlsPressed;

        /// <summary>
        /// Raised as the player steps a slider by a direction, before its value changes; dragging it with the pointer
        /// raises only the value.
        /// </summary>
        event Action OnSliderStepped;

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

        /// <summary>Raised when the player chooses Load.</summary>
        event Action OnLoadPressed;

        /// <summary>
        /// Greys Save out when saving is not allowed here; it stays focusable, so choosing it can say no.
        /// </summary>
        void SetSaveEnabled(bool enabled);

        /// <summary>Shows where the player is, by the place's localisation key hash.</summary>
        void SetLocationName(ulong keyHash);

        /// <summary>Shows the play time as hours and minutes, the colon lit or not as it blinks.</summary>
        void SetPlayTime(uint seconds, bool colonLit);

        /// <summary>
        /// Shows the countdown as minutes and seconds in the play time's place, while it is on screen, the colon lit or
        /// not as it blinks.
        /// </summary>
        void SetCountdown(uint seconds, bool colonLit);
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
        /// Shows a row for each save, laid out as <see cref="IMenuUIProvider.SaveSlot" />, focusing the one at
        /// <paramref name="focusIndex" />, or the first row below 0.
        /// </summary>
        void SetSlots(IReadOnlyList<SavePreview> slots, int focusIndex);

        /// <summary>The focused save's index in the list, or -1 when no save's row is focused.</summary>
        int GetFocusedSlot();

        /// <summary>Asks whether to write over the focused save.</summary>
        void AskToOverwrite();

        /// <summary>Asks whether to delete the focused save.</summary>
        void AskToDelete();

        /// <summary>Closes the question, focus going back to the row it asked about.</summary>
        void CloseQuestion();
    }
    /// <summary>The Controls page, opened from Config: the player's own bindings for the controls its layout lists.</summary>
    [MenuName(MenuIds.CONTROLS)]
    public interface IControlsMenu : IMenu
    {
    }

    /// <summary>
    /// The Controls page's UI: a button for each control in each control scheme, from the layout's
    /// <c>ControlRow</c>s, and a Reset to defaults button.
    /// </summary>
    public interface IControlsMenuUI : IMenuUI
    {
        /// <summary>Raised as the player chooses a control's button, by its place among the buttons.</summary>
        event Action<int> OnControlChosen;

        event Action OnResetPressed;

        int ButtonCount { get; }

        /// <summary>Every control the layout lists, one per row.</summary>
        IReadOnlyList<RebindableControl> Controls { get; }

        RebindableControl GetControl(int button);

        string GetScheme(int button);

        void SetControlName(int button, string name);

        /// <summary>Shows or hides the prompt to press the control to bind.</summary>
        void ShowPrompt(bool shown);
    }
}
