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
        /// TODO: This is a placeholder - needs actual game feature checking
        /// NOTE: Currently always returns false - implement with actual Underground DLC features
        /// </summary>
        protected virtual bool hasLayerTransitionAccess(Location from, Location to)
        {
            if (from == null || to == null) return false;
            if (from.hex == null || to.hex == null) return false;

            // Check if locations are adjacent (can only transition through adjacent hexes)
            if (!from.isNeighbour(to)) return false;

            // TODO: Check for underground entrance features
            // This requires knowing the actual Underground DLC feature types
            // Examples might include: "Cave Entrance", "Mine Shaft", "Underground Access", etc.
            // 
            // Implementation would look like:
            // foreach (Feature feature in from.properties.features)
            // {
            //     if (feature is UndergroundEntrance || feature is CaveEntrance)
            //     {
            //         return true;
            //     }
            // }
            //
            // OR using the extension method with proper implementation:
            // foreach (Feature feature in from.properties.features)
            // {
            //     if (feature != null && feature.providesUndergroundAccess())
            //     {
            //         return true;
            //     }
            // }

            Debug.LogWarning("IxthusFix: Layer transition access checking not fully implemented - cannot cross layers");
            return false; // Placeholder - implement with actual game feature checks
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
        /// TODO: This is a placeholder implementation
        /// NOTE: Real pathfinding would require access to Underground DLC's transition point system
        /// </summary>
        protected virtual List<Location> findLayerTransitionPath(Location target)
        {
            // This is a simplified placeholder
            // Real implementation would use A* or similar pathfinding
            // accounting for layer transition points

            List<Location> path = new List<Location>();

            // A complete implementation would need to:
            // 1. Find nearest underground entrance from current location
            //    - Search nearby locations for features that provide underground access
            //    - Use pathfinding to reach the entrance
            // 2. Path to entrance on current layer
            // 3. Transition through the entrance (layer change)
            // 4. Path from entrance to target on the other layer
            //    - May need to search for matching entrance on other side
            //
            // Example pseudocode:
            // var entrance = findNearestLayerTransition(location);
            // if (entrance != null)
            // {
            //     var pathToEntrance = base.getPathTo(entrance);
            //     var pathFromEntrance = pathfindOnTargetLayer(entrance, target);
            //     path.AddRange(pathToEntrance);
            //     path.AddRange(pathFromEntrance);
            // }

            Debug.LogWarning("IxthusFix: Layer transition pathfinding not fully implemented - agent cannot cross layers");
            Debug.LogWarning($"IxthusFix: Would need to find path from {location?.getName() ?? "unknown"} to {target?.getName() ?? "unknown"} across layers");
            
            return path; // Empty path - cannot cross layers with current implementation
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
/// TODO: This needs to be implemented based on actual Underground DLC feature types
/// CURRENT LIMITATION: Always returns false - agents cannot cross layers
/// 
/// To implement properly, you would need to:
/// 1. Identify the actual Feature types in the game that provide underground access
/// 2. Check if the feature is one of those types
/// 
/// Examples of features that might provide access:
/// - Cave entrances
/// - Mine shafts  
/// - Dungeon entrances
/// - Natural underground passages
/// - Man-made underground structures
/// </summary>
public static class FeatureExtensions
{
    public static bool providesUndergroundAccess(this Feature feature)
    {
        // TODO: Implement actual feature type checking
        // Example implementation:
        // if (feature == null) return false;
        // 
        // return feature.GetType().Name.Contains("Cave") ||
        //        feature.GetType().Name.Contains("Mine") ||
        //        feature.GetType().Name.Contains("Underground") ||
        //        feature is SpecificUndergroundEntranceFeatureType;

        Debug.LogWarning("IxthusFix: Feature.providesUndergroundAccess() not implemented - no layer transitions possible");
        return false; // Placeholder - always denies underground access
    }
}
