# RPGFramework.Menu

The menus of an RPG Framework game, as a module with a scene of its own: the title screen and its settings, and the
menus opened from the field. Menus are UI Toolkit documents, stacked one over another.

Requires Unity 6000.7 or newer, RPGFramework.Core and its shared types, RPGFramework.Menu.SharedTypes,
RPGFramework.DI, RPGFramework.Audio, RPGFramework.Hashing, RPGFramework.Localisation and Unity.Mathematics.

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
   module share it. **Whatever opens the menu module names the menu in it first** — nothing is opened by default: your
   splash screen sets `new MenuArgs(MenuIds.Begin)` before it finishes, as the Starter's does, and the field names the
   party menu.
4. **Add the scene to your game's module and scene databases**, under `MenuConstants.MODULE_ID`.

**Or make the layouts without the sample**, from **Assets > Create > RPG Framework > Menu > Layouts**: one entry per
menu, and the save slot, or **All Menus** for every layout and a Menu UI Provider holding them, in the folder selected.
They are the sample's, copied as new files of your own. Each refers to your menu stylesheet, found wherever it is by its
first line, `/* RPG Framework menus */`, or made beside the new layout if the project has none.

To replace a menu or a menu UI with your own, bind its interface — `IBeginMenuUI`, say — in the scene's installer: the
Menu Module binds each only where nothing is bound yet.

### A menu of your own

A menu is named by the hash of its name, as `MenuIds` names the framework's — `MenuIds.Party` is `"Party"`'s. To add
one, a Status menu, say:

1. **Name it**, in a class of your own, the name and its hash (RPGFramework.Hashing) side by side so they cannot
   differ:

   ```csharp
   public const string STATUS = "Status";
   public static readonly ulong Status = Fnv1a64.Hash(STATUS);
   ```

2. **Write it as a pair**: an interface deriving `IMenu`, **declared by its name**, and a class deriving
   `Menu<IStatusMenuUI>`; an interface deriving `IMenuUI` and a class deriving `MenuUI<IStatusMenuUI>`. The menu's
   constructor passes the base its UI, the input router, the menu module, the `ISfxPlayer`, the game's
   `ISystemSoundConfig`, and Core's `IVariableMap` and `IMemoryService`, which grey its entries; it sounds the cursor
   itself, and `PlaySound(m_SystemSounds.Cancel)` plays any other. **Its entries need no code** (below): the base
   offers, greys and opens them. Override `CanOpen` for a condition of the menu's own, as the party menu greys Save
   without the save flag.

   ```csharp
   [MenuName(GameMenus.STATUS)]
   public interface IStatusMenu : IMenu { }
   ```
3. **Bind both, and add the menu to a `MenuDatabase`**, in the menu scene's installer, through its
   `IMenuDatabaseBuilder`. The database starts with the framework's menus; bound as `IMenuDatabase`, which only looks
   menus up, it replaces the one the Menu Module would bind.

   ```csharp
   public override void InstallBindings(IDIContainer container)
   {
       container.BindSingletonFromInstance<IMenuUIProvider>(m_MenuUIProvider);
       container.BindSingletonFromInstance<IMenuTransitionConfig>(m_TransitionConfig);

       container.BindTransient<IStatusMenu, StatusMenu>();
       container.BindTransient<IStatusMenuUI, StatusMenuUI>();

       MenuDatabase         menus   = new MenuDatabase();
       IMenuDatabaseBuilder builder = menus;

       builder.Add<IStatusMenu>(GameMenus.Status);
       container.BindSingletonFromInstance<IMenuDatabase>(menus);
   }
   ```

4. **Give the Menu UI Provider its layout**: once the menu is declared, the provider's inspector lists it beside the
   framework's, each with a field for its layout.
5. **Open it**: an entry naming it in another menu's layout (below), `PushMenu(GameMenus.Status)` from another menu's
   code, or `new MenuArgs(GameMenus.Status)` in `IMenuArgsStore` from another module, as the field opens the party menu.

### An entry of your own

A menu's choices are **entries in its layout**, each a `MenuEntry` — so adding a Journal to the party menu, or taking
Load away, is an edit to the layout and nothing else:

```xml
<RPGFramework.Menu.UI.MenuEntry opens="Journal" when="JournalFound" help="PartyMenu/JournalHelp" text="Journal">
    <Bindings>
        <RPGFramework.Localisation.StaticLocalisedText property="text" key="PartyMenu/Journal"/>
    </Bindings>
</RPGFramework.Menu.UI.MenuEntry>
```

- **`opens`** names the menu it opens, the framework's or your own. Choosing it plays the confirm sound and opens it.
- **`command`**, instead, names work the menu carries out itself — the title's `NewGame` and `Quit`. A menu of your own
  carries out its commands by overriding `OnCommand`, playing its own sound, and declares each on its interface,
  `[MenuCommand("Credits")]` beside its `[MenuName]`, so the UI Builder offers it.
- **`when`** names a Bool variable in your variable map: while it is false the entry is greyed — still on the cursor's
  path, choosing it playing the error sound — so your scripts set it as the story goes. Leave it empty to offer the
  entry always. A menu's own conditions come on top: the party menu's Save is greyed without the save flag.
- **`help`** is the key of a line saying what the entry does (below); its text, a localised text's binding, as any
  label's.
- An entry shows its own text, and holds whatever else you put in it.
- **`default-focus`** marks the entry the cursor starts on, while it is offered; otherwise it starts on the first. The
  cursor moves through a menu's entries in the layout's order, wrapping. They are checked again each time a menu over
  it closes, so an entry a menu turns on shows offered on the way back.
- An entry with neither `opens` nor `command` is its menu's own to handle, as Config's rows are, its sliders and the
  language taking left and right.
- **Nothing is typed in the UI Builder**: `opens` is a list of the declared menus, `command` of the commands the menus
  using the open layout declare (by your Menu UI Providers; none for a layout no menu has yet), `when` of your variable
  map's single Bools, and `help` of your sheets' keys. A name a list no longer holds is kept as it was, and checked
  (below).

### A help line

Put a label of the class **`rpg-help-line`** in a menu's layout, and it shows the `help` of the entry the cursor is on,
in the player's language, empty for an entry with none. A layout without one shows no help. `Menus.uss` places the
sample's across the top of the party menu and Config.

**The names are checked before a menu opens.** `[MenuName]` is read only in the editor and left out of a player. The
provider's inspector shows a declared menu with no layout, a layout no menu declares (with a button removing them) and
a missing save slot. It shows, too, what a layout names that your project does not hold: an entry opening a menu nothing
declares, naming a command its menu does not declare, doing both, greyed by anything but a single Bool outside
Temp, or with a help key none of your sheets holds; and a save slot showing a value that is not a persistent variable,
or a `SaveSlotText` showing one that is not a `ULong`. The menu scene is checked the same way as it loads in play mode,
logging what is wrong, and a player build fails on it. As the menu module enters in play mode, every menu in the database is checked too: one added
but never declared, or added under another menu's id, is logged, and with a UI provider of your own rather than the
asset, so is a menu it has no layout for.

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
  `VisualTreeAsset` the **Menu UI Provider** asset holds under the menu's name (Create > RPG Framework > Menu > Menu UI
  Provider): its inspector lists every declared menu — `Begin`, `Config`, `Language`, `Controls`, `Party`, `Save` and
  `Load` for the framework's, Save and Load sharing the sample's save slots layout, and any of your own. The package ships no layouts of its
  own: the sample's are the starting point. A layout keeps the names of the elements
  its UI looks for — Config's `MusicVolumeBtn`, `SaveTitleLabel` — and its text's keys are its own; its entries need
  no names.
- **The cursor moves the same way by key, pad or pointer**, between each menu's buttons, with the cursor sound, and
  nothing in a menu reacts while the screen fades. A button is any element the menu links (Core's `SelectionScope`):
  the sample's are plain labels, found by name. The hand is Core's `.rpg-selectable:focus`, which your stylesheet can
  restyle. A menu and its UI are bound transient, as the sample binds them:
  each menu listens to its own UI.
- **Sounds are your game's system sounds** — the cursor, a confirm, a cancel, an error, a new game, a load and a save
  — which your game picks in Core's **System Sound Config**. Choosing an entry plays the confirm sound, moving between
  them the cursor sound. The menus play them, never their UIs: a UI reports what the player did and holds no audio
  player, so a menu UI of your own plays nothing either.
- **Settings are read and written through Core's settings store** and written to its settings file, never the save,
  and the music and sound effect volumes are applied as the title opens.

---

## The menus

- **Begin**: the title screen. Its choices are entries in its layout, so you can add your own — Credits, a menu of
  yours — or leave Quit out, as a console game has none.
  - **New Game** (`command="NewGame"`) begins a save in memory, writing nothing until the player saves, and starts at once in the module
    your variable map names as the start.
  - **Load Game** opens the Load menu, and is greyed while there is no save to load. The cursor starts on it when there
    is one (`default-focus`), and on New Game when there is not.
  - **Settings** opens Config, so the player can set their preferences before starting or loading a game; closing it
    comes back to the title. Its label is `Generic/Settings` in the sample's layout, as the Party menu's is.
  - On the very first launch, before the player has chosen a language, it shows the Language menu first, starting on
    the device's language or the nearest the game ships. Afterwards it applies the language the player chose.
  - **Debug Room**, between Settings and Quit, only where `ENABLE_RPG_FRAMEWORK_DEBUG` is defined, begins a save in
    memory as New Game does and opens the Debug Room package's room through `DebugMenuOutcomes.DEBUG_ROOM`. The
    entry is added in code, before Quit's or after the last, so your layout needs nothing for it.
  - **Quit** (`command="Quit"`) plays the cancel sound out and quits.
- **Controls**, opened from Config's Controls button: each control's key and pad button, chosen to rebind by pressing
  the new one, waiting a few seconds cancelling, a clash swapped, and Reset to defaults. **Its layout lists the
  controls**: a `ControlRow` per control, naming the actions it rebinds as one (`actions="Player/Confirm, UI/Submit"`)
  and, for a direction, its `part` (`up`), with a button per control scheme named for the scheme — add a row for an
  action of your own.
- **Config**: music and sound effect volumes and the message speeds, each volume heard live as its slider moves, and
  written when the menu closes.
- **Language**: every language the game ships, written when the menu closes.
- **Party**: the menu opened from the field, showing Config, Save and Load, with the location name and a live clock:
  the play time as hours and minutes, or the countdown as minutes and seconds in its place while it is on screen, the
  colon blinking once a second. Its choices are entries in its layout, with a help line: Save is offered only when the
  field allows saving, and is greyed out otherwise. Loading from here leaves the field under the menu behind and enters
  the save's.
- **Save** and **Load**: one list of save slots, each previewed without loading it, newest first. Save also offers a
  new slot. Overwriting or deleting a save asks first, with No selected. A save made by a newer version of the game, or
  a damaged one, is listed and marked, and can't be loaded.
  - **Each slot has a layout of its own**, the **Save Slot** on your Menu UI Provider. The sample's shows the
    location, the play time, the money and its name, and when it was saved.
  - **Show any saved value by naming its variable** in the layout, as your variable map lists it: `Money`, `Items[3]`,
    `Party[0].Level`, or `Party.Level` for a single record. **`SaveSlotNumber`** shows a number, **`SaveSlotPlayTime`** hours and minutes,
    **`SaveSlotText`** a text by its localisation key's hash, such as `LocationName`, naming the sheet it is in, and
    **`SaveSlotDate`** when it was saved, in the game's language. Fixed text is a label with a `StaticLocalisedText` —
    what your money is called, say, `Generic/Money` reading "Gil" or "Dollars". What is inside a
    **`SaveSlotShownWhen`** shows only for a damaged save, or only for a newer one, as its `problem` says — the
    sample's holds the text saying so — and what is inside a **`SaveSlotDetails`** is hidden for a damaged save.
  - **Anything else is an element of your own** implementing `ISaveSlotElement`, which is handed each save to show:
    the party's portraits, say, from sprites it references in the layout.
