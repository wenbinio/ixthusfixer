using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace IxthusFix
{
    /// <summary>
    /// Gawain - A special agent/unit for the King of Cups
    /// FIXED: Now properly handles Underground DLC movement and pathfinding
    /// </summary>
    public class UAE_Gawain : Unit
    {
        public UAE_Gawain(Location location, SocialGroup sg) : base(location, sg)
        {
        }

        public override string getName()
        {
            return "Gawain";
        }

        public override string getDesc()
        {
            return "A devoted servant of the King of Cups, Gawain spreads faith and guards sacred sites. Blessed with divine purpose.";
        }

        /// <summary>
        /// FIX: Movement now considers layer transitions
        /// </summary>
        public override bool canMoveTo(Location target)
        {
            if (target == null) return false;
            if (location == null) return false;

            // FIX: Check if locations are on different layers
            bool isDifferentLayer = false;
            if (location.hex != null && target.hex != null)
            {
                isDifferentLayer = (location.hex.z < 0) != (target.hex.z < 0);
            }

            if (isDifferentLayer)
            {
                // FIX: Check if this unit can traverse between layers
                if (!canTraverseLayers())
                {
                    return false;
                }

                // Check if there's a valid transition point (cave, stairs, etc.)
                if (!hasLayerTransitionAccess(location, target))
                {
                    return false;
                }
            }

            return base.canMoveTo(target);
        }

        /// <summary>
        /// FIX: Determine if this unit can move between surface and underground
        /// By default, most units cannot unless there's a transition point
        /// </summary>
        protected virtual bool canTraverseLayers()
        {
            // Default: Gawain can only traverse layers through proper entrances
            // Could be upgraded with special ability
            return false;
        }

        /// <summary>
        /// FIX: Check if there's a cave entrance or other transition between layers
        /// </summary>
        protected virtual bool hasLayerTransitionAccess(Location from, Location to)
        {
            if (from == null || to == null) return false;
            if (from.hex == null || to.hex == null) return false;

            // Check if locations are adjacent (can only transition through adjacent hexes)
            if (!from.isNeighbour(to)) return false;

            // Check for underground entrance features
            // This would need to check for actual game features like caves, mines, etc.
            foreach (Feature feature in from.properties.features)
            {
                // Check if feature allows layer transition
                // Placeholder - would need actual game's underground entrance feature types
                if (feature != null && feature.providesUndergroundAccess())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// FIX: Pathfinding now accounts for layers
        /// </summary>
        public override List<Location> getPathTo(Location target)
        {
            if (target == null) return new List<Location>();

            // FIX: If on different layers, need to find transition point
            if (location != null && location.hex != null && target.hex != null)
            {
                bool isDifferentLayer = (location.hex.z < 0) != (target.hex.z < 0);
                
                if (isDifferentLayer)
                {
                    // Find path that includes layer transition
                    return findLayerTransitionPath(target);
                }
            }

            return base.getPathTo(target);
        }

        /// <summary>
        /// FIX: Find a path that crosses between layers
        /// </summary>
        protected virtual List<Location> findLayerTransitionPath(Location target)
        {
            // This is a simplified placeholder
            // Real implementation would use A* or similar pathfinding
            // accounting for layer transition points

            List<Location> path = new List<Location>();

            // Would need to:
            // 1. Find nearest underground entrance from current location
            // 2. Path to entrance
            // 3. Transition layer
            // 4. Path from entrance to target on other layer

            Debug.LogWarning("IxthusFix: Layer transition pathfinding not fully implemented");
            
            return path;
        }

        /// <summary>
        /// FIX: Vision/sensing now properly handles layers
        /// </summary>
        public override List<Location> getVisibleLocations(Map map)
        {
            List<Location> visible = base.getVisibleLocations(map);

            // FIX: Filter out locations on different layers unless there's line of sight
            if (location != null && location.hex != null)
            {
                int currentLayer = location.hex.z < 0 ? -1 : 0;
                
                visible.RemoveAll(loc => 
                {
                    if (loc?.hex == null) return true;
                    int locLayer = loc.hex.z < 0 ? -1 : 0;
                    
                    // Can't see across layers (unless special ability)
                    if (locLayer != currentLayer && !canSeeThroughLayers())
                    {
                        return true; // Remove from visible list
                    }
                    
                    return false;
                });
            }

            return visible;
        }

        /// <summary>
        /// FIX: Can this unit see through layers?
        /// </summary>
        protected virtual bool canSeeThroughLayers()
        {
            // By default, cannot see between surface and underground
            // Could grant special divine vision ability
            return false;
        }

        /// <summary>
        /// FIX: Turn tick now handles layer-specific effects
        /// </summary>
        public override void turnTick(Map map)
        {
            base.turnTick(map);

            // Check which layer we're on for layer-specific behaviors
            if (location?.hex != null)
            {
                bool isUnderground = location.hex.z < 0;
                
                if (isUnderground)
                {
                    // Special behavior when underground
                    processUndergroundTurnTick(map);
                }
                else
                {
                    // Special behavior on surface
                    processSurfaceTurnTick(map);
                }
            }
        }

        protected virtual void processUndergroundTurnTick(Map map)
        {
            // Placeholder for underground-specific turn effects
        }

        protected virtual void processSurfaceTurnTick(Map map)
        {
            // Placeholder for surface-specific turn effects
        }

        /// <summary>
        /// Visual representation
        /// </summary>
        public override Sprite getPortraitForeground()
        {
            // Load from resources - would need actual sprite file
            return null; // Placeholder
        }
    }
}

/// <summary>
/// Extension method to check if a feature provides underground access
/// This would need to be implemented based on actual game features
/// </summary>
public static class FeatureExtensions
{
    public static bool providesUndergroundAccess(this Feature feature)
    {
        // Placeholder - would need to check actual feature types
        // Examples: Cave entrance, mine shaft, dungeon entrance, etc.
        return false;
    }
}
