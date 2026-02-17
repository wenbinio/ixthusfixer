# IxthusFix - Underground DLC Compatibility Guide

## What This Fix Does

This is a template/reference implementation showing how to fix the Ixthus mod for Underground DLC compatibility. The original mod breaks because it doesn't account for the new underground layer system introduced in the DLC.

## Key Changes Made

### 1. Settlement Layer Support (Set_Crypt.cs)

**Problem**: Crypts didn't specify which layer they could be placed on, causing crashes during map generation.

**Solution**: Implemented layer validation methods:
```csharp
public override bool isLayerValid(int layer)
{
    return layer == 0; // Surface only
}

public override bool isUnderground()
{
    return false;
}

public override bool canBeOnSurface()
{
    return true;
}

public override bool canBeUnderground()
{
    return false;
}
```

**Why It Works**: The game now knows crypts should only spawn on the surface layer (layer 0), preventing placement errors.

### 2. Agent Movement and Pathfinding (UAE_Gawain.cs)

**Problem**: Agents couldn't handle movement between surface and underground layers.

**Solution**: Implemented layer-aware movement:
```csharp
public override bool canMoveTo(Location target)
{
    // Check if moving between layers
    bool isDifferentLayer = (location.hex.z < 0) != (target.hex.z < 0);
    
    if (isDifferentLayer)
    {
        // Verify layer transition is possible
        if (!hasLayerTransitionAccess(location, target))
        {
            return false;
        }
    }
    
    return base.canMoveTo(target);
}
```

**Why It Works**: Agents now check if they're crossing layers and whether a transition point (cave entrance, etc.) exists.

### 3. God Powers and Targeting (God_KingofCups.cs)

**Problem**: Powers didn't validate target locations for layer compatibility.

**Solution**: Added layer-aware targeting:
```csharp
protected virtual bool isValidTarget(Location location)
{
    if (location == null) return false;
    
    // Can target both layers (or add restrictions as needed)
    return true;
}

public override void executePower(Location target, int powerIndex)
{
    if (target == null) return;
    
    // Check layer
    bool isUnderground = target.hex != null && target.hex.z < 0;
    
    // Handle accordingly
    if (isUnderground)
    {
        // Special underground logic
    }
}
```

**Why It Works**: Powers now properly validate and handle targets on both surface and underground layers.

### 4. Vision and Sensing

**Problem**: Units could "see" through layers inappropriately.

**Solution**: Filtered visible locations by layer:
```csharp
public override List<Location> getVisibleLocations(Map map)
{
    List<Location> visible = base.getVisibleLocations(map);
    
    int currentLayer = location.hex.z < 0 ? -1 : 0;
    
    visible.RemoveAll(loc => {
        int locLayer = loc.hex.z < 0 ? -1 : 0;
        return locLayer != currentLayer; // Remove if on different layer
    });
    
    return visible;
}
```

**Why It Works**: Units can now only see locations on their current layer, preventing X-ray vision through the ground.

## How to Use This Fix

### For Players

1. **If you have the original Ixthus mod**:
   - Disable the original Ixthus mod
   - Enable IxthusFix instead
   - Start a new game (existing saves with original Ixthus may not work)

2. **Building the mod** (if you have the source):
   ```
   1. Open IxthusFix.sln in Visual Studio
   2. Update Assembly-CSharp.dll reference to point to your game installation
   3. Build in Release mode
   4. Copy output to: %AppData%\..\LocalLow\Fallen Oak Games\Shadows of Forbidden Gods\Mods\IxthusFix\
   ```

### For Mod Developers

Use this as a reference for fixing your own mods. Key patterns:

1. **Always implement layer methods in Settlement classes**
2. **Check `location.hex.z` to determine layer** (< 0 = underground, >= 0 = surface)
3. **Filter neighbor/range operations by layer**
4. **Validate transitions between layers**

## Testing Checklist

- [ ] Mod loads without errors
- [ ] Game starts with mod enabled
- [ ] Map generates successfully
- [ ] Crypts spawn on surface
- [ ] Gawain can move normally on surface
- [ ] Gawain cannot phase through ground to underground
- [ ] God powers work on surface locations
- [ ] God powers handle underground locations (if applicable)
- [ ] Save/load works correctly
- [ ] No null reference exceptions in logs

## Game Startup Review (Failure/Malfunction Points)

How the game typically calls this mod:
1. The game reads `mod_desc.json`
2. It loads `IxthusFix.dll`
3. It instantiates `IxthusFix.ModKernel` (inherits `ModKernelAbstract`)
4. It calls `onStartup(Map map)` to register content (for this mod: `map.addGod(new God_KingofCups())`)

Startup risks reviewed in `ModKernel.onStartup`:
- **Duplicate startup calls**: If startup is invoked more than once in a session, duplicate registration can occur. The kernel now guards against re-running initialization.
- **Null map input**: If the game calls startup before map allocation is complete or after a failed load path, `map` can be null. The kernel now exits safely and logs a clear failure reason.
- **Partial initialization**: If `map.addGod(...)` throws, startup remains incomplete and is not marked as successful.
- **Error visibility**: Exceptions are logged with message and stack trace to aid diagnosis.

Questions to raise during integration testing:
1. Is `onStartup` guaranteed to be called exactly once per game process, or once per new game/load?
2. Does the engine expect mods to register only on new-game creation, or also on loading save files?
3. Should startup failures hard-fail mod loading, or is log-and-continue the expected contract?
4. Are custom settlements/agents expected to be explicitly registered in startup, or discovered through other game systems?
5. When an incompatible mod is present, does the engine enforce `getIncompatibilities()` at load time or only warn?

## Common Issues and Solutions

### Issue: "Null Reference Exception" on startup
**Cause**: Missing reference to Assembly-CSharp.dll or Unity dlls
**Solution**: Update project references to point to your game installation

### Issue: Mod doesn't appear in game
**Cause**: DLL not in correct folder or mod_desc.json missing
**Solution**: Ensure both IxthusFix.dll and mod_desc.json are in the Mods folder

### Issue: Crypts still won't spawn
**Cause**: Location validation may be too strict
**Solution**: Check `isLocationValid()` method in Set_Crypt.cs

### Issue: Movement errors with Gawain
**Cause**: Pathfinding not fully implemented for layer transitions  
**Solution**: The template has placeholder pathfinding for same-layer movement

**Known Limitation**: Agents cannot currently cross between surface and underground layers because:
1. The `hasLayerTransitionAccess()` method needs actual Underground DLC feature type identification
2. The `findLayerTransitionPath()` is a placeholder implementation
3. Requires knowledge of the specific Feature types in Underground DLC (cave entrances, mine shafts, etc.)

To implement fully, you would need to:
- Examine Underground DLC features using a decompiler or modding tools
- Identify which Feature types provide layer transitions
- Implement proper feature type checking in the extension method
- Build complete A* pathfinding that accounts for transition points

## Technical Details

### Layer System

The Underground DLC uses a Z-coordinate system:
- **Surface**: `hex.z >= 0` (typically 0)
- **Underground**: `hex.z < 0` (typically -1)

### Transition Points

Locations can transition between layers through special features:
- Cave entrances
- Mine shafts
- Dungeon entrances
- Other underground access points

The game determines these during map generation. Your mod must respect them.

### Performance Considerations

Layer checking adds minimal overhead, but:
- Cache layer checks when possible
- Don't check every frame - use turn-based updates
- Filter large location lists by layer early

## Future Enhancements

This template could be extended with:
- [ ] Underground crypt variant
- [ ] Layer-crossing divine powers
- [ ] Gawain ability to bless underground entrances
- [ ] Faith spreading across layers
- [ ] Underground-specific events

## Contributing

If you improve this fix:
1. Test thoroughly
2. Document your changes
3. Submit a pull request

## Credits

- Original Ixthus mod: [Original Author Name]
- Underground DLC compatibility fix: Community
- Template structure: Based on Fallen Oak Games modding examples

## License

This fix template is provided as-is for the community. Respect the original mod's license and the game's modding guidelines.
