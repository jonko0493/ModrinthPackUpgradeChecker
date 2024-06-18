# Modrinth Pack Upgrade Checker

This is a very simple command line tool that checks to see which mods can be upgraded to a new Minecraft version in a given Modrinth mrpack.

Usage is self documented with `ModrinthPackUpgradeChecker --help` but there are two possible commands:
* `ModrinthPackUpgradeChecker -p path/to/pack.mrpack` &ndash; this will report the highest version available for each mod (see caveats below)
* `ModrinthPackUpgradeChecker -p path/to/pack.mrpack -v minecraft_version [-i]` &ndash; this will report how many mods are compatible with the specified minecraft version. Including `-i` will show compatible mods,
    while excluding it will show incompatible mods.

Examples:
```
# Show a list of mods incompatible with version 1.21
ModrinthPackUpgradeChecker -p /path/to/pack.mrpack -v 1.21

# Show a list of mods compatible with version 1.20.4
ModrinthPackUpgradeChecker -p /path/to/pack.mrpack -v 1.20.4 -i 
```

## Caveats/Known Issues
* Snapshot versions are not supported at this time.
* You can get rate limited by Modrinth if you run this too much.