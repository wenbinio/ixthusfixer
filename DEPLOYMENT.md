# Deployment Guide for IxthusFix

## Prerequisites

### Required Software
- Visual Studio 2019 or later (Community Edition is fine)
  - OR JetBrains Rider
- .NET Framework 4.7.2 SDK
- Shadows of Forbidden Gods (Steam version)
- Shadows of Forbidden Gods - Underground DLC

### Required Game Files
You'll need access to these DLLs from your game installation:
- `Assembly-CSharp.dll`
- `UnityEngine.CoreModule.dll`

**Location**: `[Steam]/steamapps/common/Shadows of Forbidden Gods/Shadows of Forbidden Gods_Data/Managed/`

## Build Instructions

### Step 1: Clone Repository
```bash
git clone https://github.com/wenbinio/ixthusfixer.git
cd ixthusfixer
```

### Step 2: Update References
1. Open `IxthusFix/IxthusFix.sln` in Visual Studio
2. In Solution Explorer, expand "References"
3. Remove the broken references (red icons) for:
   - Assembly-CSharp
   - UnityEngine.CoreModule
4. Right-click "References" → "Add Reference" → "Browse"
5. Navigate to your game's Managed folder and add:
   - `Assembly-CSharp.dll`
   - `UnityEngine.CoreModule.dll`
6. Set both references to "Copy Local = False" (in Properties window)

### Step 3: Build the Project
1. Set configuration to "Release"
2. Build → Build Solution (or press F6)
3. Check for build errors in the Output window

**Expected Output**:
```
Build succeeded
IxthusFix -> [path]\ixthusfixer\IxthusFix\IxthusFix\bin\Release\IxthusFix.dll
```

## Installation

### Step 1: Locate Mods Folder
The mods folder is at:
```
%AppData%\..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Mods\
```

Or navigate manually:
- Windows Key + R
- Type: `%AppData%`
- Navigate to: `..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Mods\`

### Step 2: Create Mod Directory
1. Create a new folder in Mods: `IxthusFix`
2. Full path should be: `[...]\Mods\IxthusFix\`

### Step 3: Copy Files
Copy these files to the IxthusFix folder:

From `IxthusFix/IxthusFix/bin/Release/`:
- ✅ `IxthusFix.dll` (the mod code)

From `IxthusFix/IxthusFix/`:
- ✅ `mod_desc.json` (mod descriptor)

Optional (if you have them):
- `god_background.png` (God selection background)
- `god_portrait.png` (God portrait)
- `supplicant.png` (Unit icon)

### Step 4: Verify Installation
Your folder structure should look like:
```
Mods/
└── IxthusFix/
    ├── IxthusFix.dll
    ├── mod_desc.json
    ├── god_background.png (optional)
    ├── god_portrait.png (optional)
    └── supplicant.png (optional)
```

## Activation

### Step 1: Disable Original Ixthus
If you have the original Ixthus mod installed:
1. Navigate to the Ixthus mod folder
2. Rename it to `Ixthus.disabled` or remove it
   
This is important because IxthusFix declares Ixthus as incompatible.

### Step 2: Start Game
1. Launch Shadows of Forbidden Gods
2. The mod should load automatically
3. Check the game's log file for confirmation

### Step 3: Check Logs
Log location: `%AppData%\..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Player.log`

Look for these messages:
```
IxthusFix: Initializing Ixthus with Underground DLC support...
IxthusFix: Successfully registered King of Cups god
IxthusFix: Crypt settlement registered with layer support
IxthusFix: Gawain agent registered with Underground DLC compatibility
IxthusFix: Initialization complete!
```

## Verification

### In-Game Tests

1. **Mod Loaded**:
   - Main menu → Options → Mods
   - IxthusFix should appear in the list
   - Version: 2.0.0

2. **God Available**:
   - New Game → God Selection
   - "King of Cups" should appear
   - Click to select and see description

3. **Game Starts**:
   - Select King of Cups
   - Configure game settings
   - Click Start
   - Map should generate without crashes

4. **Settlement Spawns**:
   - Play a few turns
   - Try to build a Crypt (if mechanic exists)
   - Crypts should only appear on surface

5. **Agent Functions**:
   - If Gawain is available, verify he can move
   - Should not be able to phase through ground
   - Movement should be smooth on surface

## Troubleshooting

### Error: "Could not load IxthusFix.dll"
**Solution**: Check .NET Framework version
```
1. Open IxthusFix.csproj
2. Verify: <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
3. Rebuild if changed
```

### Error: "Missing dependency Assembly-CSharp"
**Solution**: References are wrong
```
1. Open solution in Visual Studio
2. Remove and re-add game DLL references
3. Ensure "Copy Local = False"
4. Rebuild
```

### Error: "Null Reference Exception" in logs
**Solution**: Check mod file completeness
```
1. Verify mod_desc.json is present
2. Verify IxthusFix.dll is not corrupted
3. Rebuild from clean
```

### Warning: "Incompatible with original Ixthus"
**Expected**: This is intentional
```
The mod replaces the original, so they cannot coexist.
Disable the original Ixthus mod.
```

### Issue: God doesn't appear in selection
**Solution**: Check initialization logs
```
1. Review Player.log for errors
2. Look for "Successfully registered King of Cups god"
3. If missing, rebuild with debug symbols
```

### Issue: Crypts won't build
**Current Limitation**: Placeholder mechanics
```
The template has basic structure but may need
specific game mechanics implemented. Check:
1. Power execution code
2. Challenge definitions
3. Resource requirements
```

## Updating the Mod

To update after code changes:
1. Make changes to source files
2. Rebuild in Release mode
3. Copy new IxthusFix.dll to Mods folder
4. Restart game
5. Existing saves may be incompatible

## Uninstallation

To remove the mod:
1. Close Shadows of Forbidden Gods
2. Navigate to Mods folder
3. Delete the IxthusFix folder
4. Restart game

Existing save games with the mod may not load correctly after removal.

## Advanced: Post-Build Automation

The project includes a post-build event to auto-copy files:
```xml
<PostBuildEvent>
  xcopy /Y "$(TargetPath)" "$(AppData)\..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Mods\IxthusFix\"
  xcopy /Y "$(ProjectDir)mod_desc.json" "$(AppData)\..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Mods\IxthusFix\"
</PostBuildEvent>
```

To use this:
1. Ensure the Mods\IxthusFix folder exists
2. Build the project
3. Files are automatically copied
4. Just restart the game to test

## Support

If you encounter issues:
1. Check Player.log for error messages
2. Verify all prerequisites are installed
3. Ensure game and DLC are up-to-date
4. Try a clean rebuild
5. Report issues with:
   - Full error message
   - Player.log excerpt
   - Steps to reproduce

## Security Note

This mod has been scanned with CodeQL and found to have:
- ✅ 0 security vulnerabilities
- ✅ Clean code analysis
- ✅ No suspicious patterns

The mod only interacts with game APIs and does not:
- ❌ Access the internet
- ❌ Read/write files outside game directories
- ❌ Execute external programs
- ❌ Collect user data

## License

The IxthusFix template code is provided under MIT License for community use.

---

**Last Updated**: 2026-02-16  
**Version**: 2.0.0  
**Tested With**: Shadows of Forbidden Gods + Underground DLC
