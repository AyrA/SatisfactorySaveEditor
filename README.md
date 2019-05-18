# Satisfactory Savegame Editor

This application can read, modify and write Save games for Satisfactory

## File Format

The file format is not too difficult to read. Numbers are little endian and most values are prefixed.
String handling is somewhat special because strings are length prefixed and null terminated.
Normally you only do one of those things.
The editor removes the null terminator on load and ads it back when writing.

It looks like the file has to be in a certain order.
The string list has to be last for example

## `GENERAL WARNING`

Please make a backup of your save files.
The editor will warn you before overwriting but you should still backup your saves.
Saves are in `%LOCALAPPDATA%\FactoryGame\Saved\SaveGames`

When overwriting, a .bak file is created **if none does exist already**

## Important and Missing Functions

These important functions are not yet coded in (see TODO list at bottom for more stuff):

- Player teleporting
- Editing and linking inventories; Demo of linked inventory: https://www.youtube.com/watch?v=TtXlkJa8l_k
- Unlocking technologies
- Unlocking space elevator tiers
- Spawning Lizard Doggos

## Available Utility Functions

Various utility functions are already programmed in. Note: They have no effect on your inventory, essentially allowing you to duplicate items:

- Removing **tamed** Lizard Doggos (or space rabits as they are called internally)
- Restore the big rocks that were removed by the player (untested)
- Remove rocks (not yet working because the player can't remove them yet)
- Restore **all** plants that the player removed by any means
- Restore pickups around drop pods
- Restore berries, nuts and mushrooms
- Restore drop pods. This will not remove any harddrives you possess already
- Restore artifacts
- Remove parts from dead animals you forgot to pick up.
- Remove neutral animals
- Remove hostile animals
- Resize doggos
- Restore power slugs

# TODO

- [X] Reading player position
- [ ] Teleporting objects
- [X] Reading coordinates of objects
- [ ] Editing inventories
- [ ] Linking inventories
- [ ] Unlocking technologies
- [ ] Unlocking space elevator tiers
- [ ] Spawning Lizard Doggos
- [ ] Make sure edits are valid
- [X] UI
- [ ] Multi player capability (handling of multiple player entities)
