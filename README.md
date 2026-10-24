# RPGFramework.Menu

The menus of an RPG Framework game, as a module with a scene of its own: the title screen and its settings, and the
menus opened from the field. Menus are UI Toolkit documents, stacked one over another.

Requires Unity 6000.6 or newer, RPGFramework.Core and its shared types, RPGFramework.Menu.SharedTypes,
RPGFramework.DI, RPGFramework.Audio, RPGFramework.Localisation and Unity.Mathematics.

---

## Getting started

Import the **Menu Sample** from the Package Manager. It is a menu scene with:

- an installer binding every menu, its UI and its localisation;
- a UI provider holding each menu's UXML;
- a transition config for menu changes, using your game's default transitions;
- the `InputAdapter` the menu module finds on entering, with its actions left for you to assign.

Then make it your own:

1. **Assign the Input Adapter's actions.** Deleting a save uses the Tertiary control, so give it an action.
2. **Pass your own localisation keys to the installer.** The sample writes keys from an example spreadsheet as literals;
   your game passes its own, or its generated keys constants.
3. **In your global installer, bind `IMenuArgsStore` to `MenuArgsStore`**, so whatever opens the menus and the menu
   module share it.
4. **Add the scene to your game's module and scene databases**, under `MenuConstants.MODULE_ID`.

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
  `VisualTreeAsset` the **Menu UI Provider** asset holds for it (Create > RPG Framework > Menu > Menu UI Provider), and
  given its text through its own localisation arguments.
- **The cursor moves the same way by key, pad or pointer**, between each menu's buttons, with the cursor sound, and
  nothing in a menu reacts while the screen fades. A menu and its UI are bound transient, as the sample binds them:
  each menu listens to its own UI.
- **Sounds are requested by what happened**, such as navigating, cancelling or saving, through Core's audio intents, so
  your game decides which sound each one is. The menus play them, never their UIs: a UI reports what the player did
  and holds no audio player, so a menu UI of your own plays nothing either.
- **Settings are read and written through Core's settings service**, never the save, and the music and sound effect
  volumes are applied as the game starts.

---

## The menus

- **Begin**: the title screen.
  - **New Game** begins a save in memory, writing nothing until the player saves, and starts in the module your variable
    map names as the start.
  - **Load Game** opens the Load menu.
  - On the very first launch, before any settings have been saved, it shows the Language menu first, starting on the
    device's language or the nearest the game ships. Afterwards it applies the saved language, unless a later build
    no longer ships it, when the game stays in the device's or the nearest.
  - **Debug Room**, above Quit, only where `ENABLE_RPG_FRAMEWORK_DEBUG` is defined, begins a save in memory as New Game
    does and opens the Debug Room package's room through `DebugMenuOutcomes.DEBUG_ROOM`. The button is added in code,
    so your layout needs nothing for it.
- **Config**: music and sound effect volumes and the message speeds, each volume heard live as its slider moves, and
  written when the menu closes.
- **Language**: every language the game ships, written when the menu closes.
- **Party**: the menu opened from the field, showing Config, Save and Load, with the location name and play time. Save
  is offered only when the field allows saving, and is greyed out otherwise. Loading from here leaves the field under
  the menu behind and enters the save's.
- **Save** and **Load**: one list of save slots, each previewed without loading it, newest first, with the location,
  play time and when it was saved. Save also offers a new slot. Overwriting or deleting a save asks first, with No
  selected. A save made by a newer version of the game, or a damaged one, is listed and marked, and can't be loaded.

---

## Not in this version

- **Inventory, Abilities and CharacterInfo aren't built**, and asking for one fails.
- **Controls can't be rebound** from the Config menu.
- **The set of menus is fixed**: a game can restyle and re-word them, but can't add a menu type of its own.
