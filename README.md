# Satisfactory Savegame Editor

This application can read, modify and write Save games for Satisfactory

## Download

The latest compiled version can be obtained in the "Releases" section of this repository
or at https://cable.ayra.ch/satisfactory/editor.php

## Instructions

This readme document only gives a brief overview of the features.
The context help (`[F1]`) contains more detail about the feature you are using.
Main menu items have a tooltip that shows when you hover over it with the mouse.

## `GENERAL WARNING`

Please make a backup of your save files.
The editor will warn you before overwriting but you should still backup your saves in case your good idea turned out to be horrible.
Saves are in `%LOCALAPPDATA%\FactoryGame\Saved\SaveGames`

When overwriting, a `.sav.gz` file is created **if none does exist already**.

- To open the save file location in an explorer window, press `[CTRL]`+`[L]` in the main window or use the File menu
- To manage save files and create/restore backups, press `[CTRL]`+`[M]` in the main window or use the File menu

## Usage Help

Most menu items and labels, as well as some buttons contain tool tip texts.
They appear when you hover your cursor over them.

Otherwise the usage is generally straight forward:

1. Open file
2. Make changes using the menus
3. Save file

If you get stuck at any point, press `[F1]` to bring up the help window.

## Features

This editor is not meant to edit individual item properties (no micro managing of items).
This does not mean you will never be able to edit individual item propertied, but it's not the main focus.
An editor from a different developer exists that allows these kind of edits.

It's meant to make bulk changes to the save file and change predefined features.

A few prominent features are outlined below

### Map View

The main window will display a map with item positions overlaid on it.
The map can be refreshed by pressing `F5` at any time in the main window.
Whenever you do this, a copy of the image is saved next to the save file itself.

Some dialogs have an `M` button next to the item list.
This button allows you to render your current selection on the main window map.

### Header Editor

This allows you to change the session name and accumulated play time.

### Object Counter

This shows a list of all entries in the save file and how many of them there are.
You can render individual item groups or multiple of them to the main map.

### Duplicator

This allows you to duplicate entries.
Be aware that not all entries behave properly when duplicated,
especially those that are supposed to only exist once.

You can optionally set an offset for duplicated items.
The offset is always applied to the previously offset item when creating multiple copies at once,
essentially making a row of copies on the map.

In most cases you want to apply an offset, the entries will clip otherwise and cause lag.

### Deleter

The deleter allows you to get rid of entries quickly.
Two versions exist:

- **Delete by type**: Gets rid of a specific type of entry, either all of them, or a user defined range
- **Delete by region**: Deletes entries inside a user selected polygon

### Export/Import

The exporter allows you to export individual entries or groups of entries to the clipboard to import them later into a different save file.

The export happens in XML format.
You can paste the contents into any text editor and edit to your liking.

#### Import Options

Importing is done from the clipboard. The importer has two options.

One option allows you to delete all existing entries of the same type you import.
This can be helpful when importing entries that are supposed to only exist once,
or when you want to overwrite your factory with the one you import.

The other option allows you to fix internal names.

### Utility Functions

Utility functions have no effect on your inventory, essentially allowing you to duplicate items.

- Restore the big rocks that were removed by the player (untested)
- ~~Remove rocks~~ (not yet working)
- Restore **all** plants that the player removed by any means
- Restore pickups around drop pods
- Restore berries, nuts and mushrooms
- Restore drop pods. This will not remove any harddrives you possess already
- Restore artifacts
- Restore power slugs
- Remove parts from dead animals you forgot to pick up.
- Removing **tamed** Lizard Doggos (or space rabits as they are called internally)
- Remove neutral animals
- Remove hostile animals
- Resize doggos

### Audio Extraction

If you have the game installed (any version works, weekend test, early release, experimental),
you can extract all audio files from the game files themselves.
This will only extract "RIFF WAVE" formatted audio. You will get:

- Music
- Background noise
- Sound effects
- A.D.A messages

The files have no names, so they are just numbered, starting with `0001`.
The extracted files are put in the "Audio" directory inside the editor directory.
It's created automatically once you use the Audio Extractor feature.

## Missing Functions

These functions are not yet coded in (see TODO list at bottom for more stuff):

- Player teleporting
- Editing and linking inventories; [Demo of linked inventory](https://www.youtube.com/watch?v=TtXlkJa8l_k)
- Unlocking technologies
- Unlocking space elevator tiers
- Spawning things (note: this can be partially achieved with the duplicator)

## Other Features

These features are contained in the editor but not related to editing

### Automatic Updates

The editor shows a menu option if an update is available.
You don't have to click it if you don't want to.
In that case the update is installed on the next launch.

### Contextual Help

Regardless of where you are, pressing `[F1]` brings up the help window with help for the current context.

The window is not modal, meaning you can continue to work while it's being shown.
Pressing `[F1]` in a different window will update the help window with relevant help text.

### Version History

You can access the full version history from the Help menu

### Crash Reporter

If the application is about to crash,
it allows you to send an error report.

The error report includes:

- The error message
- Environment variables
- Current contents of `lastlog.txt`
- The save file being edited

Your username is stripped from environment variables.
Variables containing `password`,`pwd` or `login` will be completely excluded from the list.

If you don't feel comfortable sending this information,
you can just close the window. Information is only sent when you press the "Send" button.

## Technical Reference

This chapter contains things you might want to know about the save file

### Internal names

Entries have two names.
One name is the one that identifies the entry.
This will exist as many times as there are entries of that type.

The other name is a kind of "internal name" that is dynamically generated and has to be unique for each object.
The internal name is used when entries reference other entries.

The importer has an option to ensure they are unique,
and the duplicator will automatically generate new names for cloned entries.

In most cases the internal name will not bother you,
but when you try to import entries, they could be changed.

### Map Coordinates

The coordinates `X=0,Y=0,Z=0` are the center of the map, not the top left corner as would be intuitive.
A single unit is 1cm. This means the 8x2 foundation has a bounding box of `X=800,y=800,Z=200`

keep this in mind when applying offsets in the duplicator or when manually edditing exported entries.

Try to not make things clip other objects because it can cause lag.

### File Format

The file format is not too difficult to read. Numbers are little endian and most values are prefixed.
String handling is somewhat special because strings are length prefixed and null terminated.
Normally you only do one of those things.
The editor removes the null terminator on load and ads it back when writing.

The file has to be in a certain order.
The string list has to be last for example.

The editor generally keeps everything in order.

# TODO

- [X] Reading and display player position
- [ ] Teleporting objects
- [X] Reading and display coordinates of objects
- [ ] Editing inventories
- [ ] Linking inventories
- [ ] Unlocking technologies
- [ ] Unlocking space elevator tiers
- [ ] Spawning Lizard Doggos and other things
- [ ] Make sure edits are valid
- [X] UI
- [ ] Multi player capability (handling of multiple player entities)
