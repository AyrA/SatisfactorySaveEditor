# Satisfactory Savegame Editor

This application can read, modify and write Save games for Satisfactory

## Download

The latest compiled version can be obtained in the "Releases" section of this repository
or at https://cable.ayra.ch/satisfactory/editor.php

## `GENERAL WARNING`

Please make a backup of your save files.
The editor will warn you before overwriting but you should still backup your saves.
Saves are in `%LOCALAPPDATA%\FactoryGame\Saved\SaveGames`

When overwriting, a .bak file is created **if none does exist already**

## Usage Help

Most menu items and labels, as well as some buttons contain tool tip texts.
They appear when you hover your cursor over them.

Otherwise the usage is generally straight forward:

1. Open file
2. Make changes using the menus
3. Save file

## Features

This editor is not meant to edit individual item properties.
It's meant to make bulk changes to the save file and change predefined features.

### Map View

The main window will display a map with item positions overlaid on it.
The map can be refreshed by pressing `F5` at any time in the main window.
Whenever you do this, a copy of the image is saved next to the save file itself.

Dialogs have an `M` button next to the item list.
This button allows you to render your current selection on the main window map.

### Header Editor

This allows you to change the session name.

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

### Export/Import

The exporter allows you to export individual entries or groups of entries to the clipboard to import them later into a different save file.

The export happens in XML format. You can paste the contents into any text editor and edit to your liking.

#### Import Options

Importing is done from the clipboard. The importer has two options.

One option allows you to delete all existing entries of the same type you import.
This can be helpful when importing entries that are supposed to only exist once,
or when you want to overwrite your factory with the one you import.

The other option allows you to fix internal names.

### Utility Functions

Utility functions have no effect on your inventory, essentially allowing you to duplicate items.

- Restore the big rocks that were removed by the player (untested)
- Remove rocks (not yet working)
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

## Missing Functions

These functions are not yet coded in (see TODO list at bottom for more stuff):

- Player teleporting
- Editing and linking inventories; [Demo of linked inventory](https://www.youtube.com/watch?v=TtXlkJa8l_k)
- Unlocking technologies
- Unlocking space elevator tiers
- Spawning things

## Technical Reference

Things you should know when editing the save file

### Internal names

Entries have two names.
One name is the one that identifies the entry. This will exist as many times as there are entries of that type.

The other name is a kind of "internal name" that is dynamically generated and has to be unique for each object.
The internal name is used when entries reference other entries.

The importer has an option to ensure they are unique, and the duplicator will automatically generate new names for cloned entries.

### Map coordinates

The coordinates `X=0,Y=0,Z=0` are the center of the map, not the top left corner as would be intuitive.
A single unit is 1cm. This means the 8x2 foundation has a bounding box of `X=800,y=800,Z=200`

keep this in mind whe napplying offsets in the duplicator or when manually edditing exported entries.

Try to not make things clip other objects because it can cause massive lag.

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
