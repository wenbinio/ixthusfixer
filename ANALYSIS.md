# Ixthus Mod Compatibility Analysis

## Problem Statement
The Ixthus mod for Shadows of Forbidden Gods has become unplayable due to the Underground DLC release. This document analyzes the likely causes and proposes solutions.

## Known Information about Ixthus
From the CommunityLib integration code, we know Ixthus contains:
- **God Class**: `God_KingofCups` (ShadowsLib namespace)
- **Settlement**: `Set_Crypt` (ShadowsLib namespace) 
- **Agent**: `UAE_Gawain` (ShadowsLib namespace)
- **Namespace**: ShadowsLib

## Underground DLC Changes
The Underground DLC for Shadows of Forbidden Gods introduced:
1. **New Underground Layer**: A subsurface layer separate from the surface layer
2. **Location System Changes**: Locations can now exist on either surface or underground layers
3. **Settlement Placement**: Settlements need to specify which layer they exist on
4. **Pathfinding Updates**: Movement and pathfinding must account for layer transitions
5. **Map Generation**: The map generator now creates both layers simultaneously

## Likely Compatibility Issues

### 1. Settlement (Set_Crypt) Layer Assignment
**Problem**: The Crypt settlement likely doesn't specify which layer it should be placed on.

**Symptoms**:
- Game crashes during map generation
- Crypts fail to spawn
- NULL reference exceptions when trying to place settlements

**Fix Required**:
```csharp
// Old code (pre-Underground)
public class Set_Crypt : Settlement
{
    // No layer specification
}

// Fixed code (post-Underground)  
public class Set_Crypt : Settlement
{
    public override bool isUnderground()
    {
        return false; // or true if crypts should be underground
    }
}
```

### 2. Agent (UAE_Gawain) Movement and Pathfinding
**Problem**: Agents may not handle layer transitions correctly.

**Symptoms**:
- Agents get stuck
- Pathfinding errors
- Can't move between surface and underground

**Fix Required**:
- Implement layer-aware pathfinding
- Add checks for underground accessibility
- Update movement AI to handle layer transitions

### 3. God Powers and Abilities
**Problem**: God abilities may target locations without considering layers.

**Symptoms**:
- Powers fail to execute
- NULL reference when targeting underground locations
- Visual effects don't display correctly

**Fix Required**:
- Add layer checks to all targeting code
- Update range calculations to be layer-aware
- Ensure underground locations can be targeted (or explicitly blocked)

## Required Code Changes

### Priority 1: Settlement Layer Support
All settlement classes must implement the underground layer system:

```csharp
public override bool isUnderground()
{
    return false; // For surface settlements
}

public override bool canBeOnSurface()
{
    return true; // If settlement can be on surface
}

public override bool canBeUnderground() 
{
    return false; // If settlement cannot be underground
}
```

### Priority 2: Location Null Checks
Add defensive checks throughout:

```csharp
// Before accessing location properties
if (location != null && location.isValid())
{
    // Safe to use location
}
```

### Priority 3: Agent AI Updates
Update agent movement to handle layers:

```csharp
public override bool canMoveTo(Location target)
{
    if (target == null) return false;
    
    // Check if we can move between layers
    if (target.isUnderground != location.isUnderground)
    {
        // Need layer transition capability
        return hasUndergroundAccess();
    }
    
    return base.canMoveTo(target);
}
```

## Testing Strategy

1. **Map Generation Test**: Verify the mod loads without crashes
2. **Settlement Placement**: Confirm crypts spawn correctly
3. **Agent Functionality**: Test Gawain can move and act normally
4. **God Powers**: Verify all abilities work on both layers
5. **Save/Load**: Ensure games can be saved and loaded

## Implementation Plan

1. Obtain or decompile the original Ixthus mod
2. Update all Settlement classes for layer support
3. Update Agent classes for layer-aware pathfinding
4. Update God class powers for layer compatibility
5. Test thoroughly with Underground DLC enabled
6. Create compatibility patch or updated mod version

## Next Steps

To proceed with fixing Ixthus, we need:
1. Access to the original mod source code or DLL
2. Understanding of specific features that are broken
3. Test environment with Shadows of Forbidden Gods + Underground DLC

## Alternative Solution: Documentation

If source code is unavailable, create a compatibility guide for Ixthus users explaining:
- What's broken and why
- Workarounds if any exist
- How to request an update from the original author
- How other modders can learn from this for their own mods
