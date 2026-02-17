using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace IxthusFix
{
    /// <summary>
    /// Crypt - A settlement type for the King of Cups god
    /// FIXED: Now properly implements Underground DLC layer system
    /// </summary>
    public class Set_Crypt : SettlementHuman
    {
        public Set_Crypt(Location location) : base(location)
        {
        }

        public override string getName()
        {
            return "Crypt";
        }

        public override string getDesc()
        {
            return "A sacred burial site that has been consecrated to serve the King of Cups. The faithful gather here to commune with the divine.";
        }

        /// <summary>
        /// FIX: CRITICAL - Specify that crypts are surface structures
        /// This is the key fix for Underground DLC compatibility
        /// </summary>
        public override bool isLayerValid(int layer)
        {
            // Crypts can only be placed on the surface layer (layer 0)
            // Underground DLC introduces layer -1 for underground
            return layer == 0;
        }

        /// <summary>
        /// FIX: Explicitly state this is not an underground settlement
        /// </summary>
        public override bool isUnderground()
        {
            return false;
        }

        /// <summary>
        /// FIX: Can be placed on surface
        /// </summary>
        public override bool canBeOnSurface()
        {
            return true;
        }

        /// <summary>
        /// FIX: Cannot be placed underground
        /// If you want crypts to be able to spawn underground, change this to true
        /// </summary>
        public override bool canBeUnderground()
        {
            return false; // Set to true if crypts should also work underground
        }

        /// <summary>
        /// Determine if this location is valid for a crypt
        /// </summary>
        public override bool isLocationValid(Location location)
        {
            if (location == null) return false;
            if (location.settlement != null) return false; // Already has a settlement

            // FIX: Check layer validity
            if (location.hex != null)
            {
                int layer = location.hex.z < 0 ? -1 : 0;
                if (!isLayerValid(layer))
                {
                    return false;
                }
            }

            // Additional placement rules (customize as needed)
            // Example: Require certain terrain, distance from other settlements, etc.
            
            return base.isLocationValid(location);
        }

        /// <summary>
        /// FIX: Turn tick now properly handles layer checking
        /// </summary>
        public override void turnTick(Map map)
        {
            base.turnTick(map);

            // Safety check for null location
            if (location == null) return;

            // Process crypt effects
            processCryptEffects(map);
        }

        protected virtual void processCryptEffects(Map map)
        {
            // Placeholder for crypt-specific turn effects
            // Example: Spread faith, generate resources, etc.

            // Make sure to check layer when affecting nearby locations
            if (location.hex != null)
            {
                foreach (Location neighbor in location.getNeighbours())
                {
                    if (neighbor == null) continue;

                    // FIX: Only affect locations on the same layer
                    if (neighbor.hex != null && neighbor.hex.z == location.hex.z)
                    {
                        // Apply crypt effects to same-layer neighbors
                        applyEffectToNeighbor(neighbor);
                    }
                }
            }
        }

        protected virtual void applyEffectToNeighbor(Location neighbor)
        {
            // Placeholder for neighbor effects
            // Implement actual game mechanics here
        }

        /// <summary>
        /// FIX: Building/construction now checks layer compatibility
        /// </summary>
        public override double getBuildProgress()
        {
            // Verify we're still on a valid layer
            if (location?.hex != null)
            {
                int currentLayer = location.hex.z < 0 ? -1 : 0;
                if (!isLayerValid(currentLayer))
                {
                    Debug.LogWarning($"IxthusFix: Crypt at {location.getName()} is on invalid layer {currentLayer}");
                    return 0.0;
                }
            }

            return base.getBuildProgress();
        }

        /// <summary>
        /// Define what challenges/tasks can be performed at crypts
        /// </summary>
        public override List<Challenge> getChallenges()
        {
            List<Challenge> challenges = base.getChallenges();

            // Add crypt-specific challenges here
            // Make sure challenges are layer-aware if they affect other locations

            return challenges;
        }

        /// <summary>
        /// Visual representation
        /// </summary>
        public override Sprite getImage()
        {
            // Load from resources - would need actual sprite file
            return null; // Placeholder
        }
    }
}
