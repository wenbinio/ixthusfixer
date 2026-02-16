# Ixthus Fixer

## Overview
This repository contains the fix/patch for the Ixthus mod to make it compatible with the Shadows of Forbidden Gods Underground DLC.

## The Problem
The Ixthus mod (featuring the King of Cups god) stopped working after the Underground DLC was released. The mod fails to load or crashes the game because it doesn't handle the new underground layer system introduced by the DLC.

## The Solution  
Since we don't have access to the original Ixthus mod source code, this repository provides:

1. **Analysis** - Documentation of what broke and why (see ANALYSIS.md)
2. **Template** - A template showing how to structure an Underground-DLC-compatible god mod
3. **Patch Instructions** - For anyone with access to the original code

## What's in this Repository

- `ANALYSIS.md` - Technical analysis of the compatibility issues
- `IxthusFix/` - Template project structure for a fixed version
- `docs/` - Additional documentation and guides

## For Mod Users

If you're trying to play Ixthus:
1. The original mod is currently broken with Underground DLC
2. Contact the original mod author for an official update
3. Or use this template as a starting point for a community fix (requires the original assets/concept approval)

## For Mod Authors

If you're experiencing similar issues with your own mods:
1. Review `ANALYSIS.md` for common Underground DLC compatibility issues
2. Check the template in `IxthusFix/` for proper layer handling
3. Key changes needed:
   - Settlement classes must implement underground layer methods
   - Agents need layer-aware pathfinding
   - God powers must handle both surface and underground targeting

## Technical Requirements

To create a working fix, you would need:
- Visual Studio 2019 or later / Rider
- .NET Framework 4.7.2
- Shadows of Forbidden Gods game files
- Underground DLC
- Original Ixthus mod assets (if recreating)

## Legal Note

This is a compatibility analysis and template only. The Ixthus mod belongs to its original creator. This repository does not contain any original Ixthus assets or code. Any fix should respect the original mod's license and intellectual property.

## Contributing

If you have access to the original Ixthus code or want to help create a compatible version:
1. Fork this repository
2. Implement the fixes described in ANALYSIS.md
3. Test thoroughly with Underground DLC
4. Submit a pull request

## Status

🔴 **Work in Progress** - Analysis complete, awaiting original mod access or community recreation

## License

Analysis and template code: MIT License  
Original Ixthus mod: Property of original creator
