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
The editor will not allow you to overwrite the file being read but you should still do it.
Saves are in `%LOCALAPPDATA%\FactoryGame\Saved\SaveGames`

## Important and Missing Functions

These important functions are not yet coded in (see TODO list at bottom for more stuff):

- Reading player position and teleporting
- Editing inventories
- Unlocking technologies
- Unlocking space elevator tiers
- Spawning Lizard Doggos

## Available Utility Functions

Various utility functions are already programmed in. Note: They have no effect on your inventory, essentially allowing you to duplicate items.

### RemoveLizardDoggos

This will remove your tamed lizard doggos you monster.
By the way, the internal name for them is "SpaceRabbit".

### RestoreRocks (untested)

This restores removed rocks.
This function can't be tested because I can't remove rocks, but if it works like the way plants do, it should restore them.

### RemoveRocks (not yet working)

This should remove rocks eventually. Because I can't yet legitimately remove them, this function is incomplete and doesn't works yet.

### RestorePlants

This restores all generic plants you removed.

### RestorePickups

This restores generic pickups, for example the debris around drop pods

### RestoreBerries

This restores berries, nuts and mushrooms

### RestoreDropPods

This resets all drop pods to the unexplored state. You can open them up again using the proper resources and then get another hard drive

### RestoreArtifacts

This restores all artifacts

### RemoveAnimalParts

This removes any dead animal parts you might have forgotten to pick up

# TODO

- [ ] Reading player position and teleporting
- [ ] Editing inventories
- [ ] Unlocking technologies
- [ ] Unlocking space elevator tiers
- [ ] Spawning Lizard Doggos
- [ ] Make sure edits are valid
- [ ] UI
- [ ] Multi player capability
