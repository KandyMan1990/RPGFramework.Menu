# RPGFramework.Menu

The menus of an RPG Framework game, as a module with a scene of its own: the title screen and its settings, and the
menus opened from the field. Menus are UI Toolkit documents, stacked one over another.

Requires Unity 6000.6 or newer, RPGFramework.Core and its shared types, RPGFramework.Menu.SharedTypes,
RPGFramework.DI, RPGFramework.Audio, RPGFramework.Localisation and Unity.Mathematics.

---

## Getting started

Import the **Menu Sample** from the Package Manager. It is a menu scene with:

- a **Menu Module** object (GameObject > RPG Framework > Menu > Menu Module), which binds every menu and its UI;
- an installer binding the UI provider and the transition config;
- the menus' layouts, in `Layouts/`, with `Menus.uss`, the stylesheet they share, and a UI provider holding them —
  imported, they are your game's own to change;
- a transition config for menu changes, using your game's default transitions;
- the `InputAdapter` the menu module finds on entering, on the `UI` map of the RPG Framework Starter sample's actions;
- the text its layouts name, in five languages: **RPG Framework > Menu > Install Sample Text** copies it into
  `StreamingAssets/Localisation`, which a sample cannot put files in itself, and refuses a project with text of its own.

It runs as the title screen of the **Starter** sample's game (RPG Framework Starter), which says how to set it up. Then
make it your own:

1. **Give the Input Adapter your own actions**, the project-wide ones, and their `UI` map. The menus read its `Submit`
   (UI Toolkit's confirm), `Cancel` and `Delete`, which deletes the save the cursor is on, so give your `UI` map a
   `Delete` button action.
2. **Make the layouts your own.** Each label and button names its localisation key with a `StaticLocalisedText`
   binding (RPGFramework.Localisation), and each menu loads the sheets its keys are in as it opens. They name the sample's
   sheets — `Generic`, `BeginMenu`, `ConfigMenu`, `PartyMenu`, `SaveMenu` and `Locations` — so with text of your own,
   choose each label's key in the UI Builder, from a list of the keys your sheets hold, and add or leave out any text
   you like. A player build refuses a layout naming a key none of your sheets holds. **`Menus.uss`** holds how they
   look — the panel, its colours and borders, each menu's sizes — so restyling every menu is one file.
3. **In your global installer, bind `IMenuArgsStore` to `MenuArgsStore`**, so whatever opens the menus and the menu
   module share it.
4. **Add the scene to your game's module and scene databases**, under `MenuConstants.MODULE_ID`.

**Or make the layouts without the sample**, from **Assets > Create > RPG Framework > Menu > Layouts**: one entry per
menu, and the save slot, or **All Menus** for every layout and a Menu UI Provider holding them, in the folder selected.
They are the sample's, copied as new files of your own. Each refers to your menu stylesheet, found wherever it is by its
first line, `/* RPG Framework menus */`, or made beside the new layout if the project has none.

To replace a menu or a menu UI with your own, bind its interface — `IBeginMenuUI`, say — in the scene's installer: the
Menu Module binds each only where nothing is bound yet.

---

## How it works

- **The menu module keeps a stack.** It opens the menu named by `IMenuArgsStore` as it enters, `PushMenu` opens another
  over it, and `PopMenu` closes the top one, suspending and resuming the menu beneath. When the stack empties, the game
  returns to the module it came from: the field, from the party menu, for instance.
- **Every menu change covers the screen** and reveals it again, with the transitions your **Menu Transition Config**
  names (Create > RPG Framework > Menu > Menu Transition Config), bound by your menu scene installer as
  `IMenuTransitionConfig`, as the sample's is: empty for your game's default transitions, or none at all, the menus then
  swapping at once.
- **Each menu is a pair**: a menu for what it does and a menu UI for how it looks, the UI built from the
  `VisualTreeAsset` the **Menu UI Provider** asset holds for it (Create > RPG Framework > Menu > Menu UI Provider). The
  package ships no layouts of its own: the sample's are the starting point. A layout keeps the names of the elements
  its UI looks for — `NewGameBtn`, `SaveTitleLabel` — and its text's keys are its own.
- **The cursor moves the same way by key, pad or pointer**, between each menu's buttons, with the cursor sound, and
  nothing in a menu reacts while the screen fades. A menu and its UI are bound transient, as the sample binds them:
  each menu listens to its own UI.
- **Sounds are requested by what happened**, such as navigating, cancelling or saving, through Core's audio intents, so
  your game decides which sound each one is. The menus play them, never their UIs: a UI reports what the player did
  and holds no audio player, so a menu UI of your own plays nothing either.
- **Settings are read and written through Core's settings store** and written to its settings file, never the save,
  and the music and sound effect volumes are applied as the title opens.

---

## The menus

- **Begin**: the title screen.
  - **New Game** begins a save in memory, writing nothing until the player saves, and starts at once in the module
    your variable map names as the start.
  - **Load Game** opens the Load menu.
  - **Settings** opens Config, so the player can set their preferences before starting or loading a game; closing it
    comes back to the title. Its label is `Generic/Settings` in the sample's layout, as the Party menu's is.
  - On the very first launch, before the player has chosen a language, it shows the Language menu first, starting on
    the device's language or the nearest the game ships. Afterwards it applies the language the player chose.
  - **Debug Room**, between Settings and Quit, only where `ENABLE_RPG_FRAMEWORK_DEBUG` is defined, begins a save in
    memory as New Game does and opens the Debug Room package's room through `DebugMenuOutcomes.DEBUG_ROOM`. The
    button is added in code, so your layout needs nothing for it.
- **Config**: music and sound effect volumes and the message speeds, each volume heard live as its slider moves, and
  written when the menu closes.
- **Language**: every language the game ships, written when the menu closes.
- **Party**: the menu opened from the field, showing Config, Save and Load, with the location name and a live clock:
  the play time as hours and minutes, or the countdown as minutes and seconds in its place while it is on screen, the
  colon blinking once a second. Save is offered only when the field allows saving, and is greyed out otherwise. Loading from here leaves the field under
  the menu behind and enters the save's.
- **Save** and **Load**: one list of save slots, each previewed without loading it, newest first. Save also offers a
  new slot. Overwriting or deleting a save asks first, with No selected. A save made by a newer version of the game, or
  a damaged one, is listed and marked, and can't be loaded.
  - **Each slot has a layout of its own**, the **Save Slot** on your Menu UI Provider. The sample's shows the
    location, the play time, the money and its name, and when it was saved.
  - **Show any saved value by naming its variable** in the layout, as scripts name it: `Money`, `Items[3]`,
    `Party[0].Level`. **`SaveSlotNumber`** shows a number, **`SaveSlotPlayTime`** hours and minutes,
    **`SaveSlotText`** a text by its localisation key's hash, such as `LocationName`, naming the sheet it is in, and
    **`SaveSlotDate`** when it was saved, in the game's language. Fixed text is a label with a `StaticLocalisedText` —
    what your money is called, say, `Generic/Money` reading "Gil" or "Dollars". What is inside a
    **`SaveSlotShownWhen`** shows only for a damaged save, or only for a newer one, as its `problem` says — the
    sample's holds the text saying so — and what is inside a **`SaveSlotDetails`** is hidden for a damaged save.
  - **Anything else is an element of your own** implementing `ISaveSlotElement`, which is handed each save to show:
    the party's portraits, say, from sprites it references in the layout.
