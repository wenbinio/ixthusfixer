using Assets.Code;
using System.Collections.Generic;
using UnityEngine;

namespace IxthusFix
{
    /// <summary>
    /// The King of Cups - A god representing faith, spirituality, and the manipulation of belief
    /// This is a template/placeholder for the actual Ixthus god implementation
    /// FIXED: Now properly handles Underground DLC features
    /// </summary>
    public class God_KingofCups : God
    {
        public God_KingofCups()
        {
            // Basic god properties
            this.powerCostFactor = 1.0; // Adjust as needed for original implementation
        }

        public override string getName()
        {
            return "King of Cups";
        }

        public override string getAdjective()
        {
            return "faithful";
        }

        public override string getCreed()
        {
            return "Through faith and devotion, the world shall be reshaped.";
        }

        public override string getDescription()
        {
            return "The King of Cups manipulates faith and spirituality, building a network of believers who will reshape the world according to divine will. Through crypts and sacred sites, the faithful gather power.";
        }

        public override Sprite getIconFore()
        {
            // Load from resources - would need actual sprite file
            return null; // Placeholder - implement with actual assets
        }

        public override Sprite getPortrait()
        {
            // Load from resources - would need actual sprite file
            return null; // Placeholder - implement with actual assets
        }

        /// <summary>
        /// FIX: Override to handle underground locations properly
        /// </summary>
        public override List<Location> getValidTargets(Map map)
        {
            List<Location> targets = new List<Location>();

            foreach (Location location in map.locations)
            {
                if (location != null && isValidTarget(location))
                {
                    targets.Add(location);
                }
            }

            return targets;
        }

        /// <summary>
        /// FIX: Check if location is valid target considering layer
        /// </summary>
        protected virtual bool isValidTarget(Location location)
        {
            if (location == null) return false;

            // Can target both surface and underground locations
            // Add any additional validation logic here
            return true;
        }

        /// <summary>
        /// FIX: Power execution now handles both surface and underground
        /// </summary>
        public override void executePower(Location target, int powerIndex)
        {
            if (target == null)
            {
                Debug.LogWarning("IxthusFix: Attempted to execute power on null location");
                return;
            }

            // Check if location is underground
            bool isUnderground = target.hex != null && target.hex.z < 0;
            
            if (isUnderground)
            {
                // Special handling for underground locations if needed
                Debug.Log($"IxthusFix: Executing power on underground location at {target.getName()}");
            }

            // Execute power based on powerIndex
            // This is a placeholder - implement actual power logic
            switch (powerIndex)
            {
                case 0:
                    // Example: Spread faith
                    executePower_SpreadFaith(target);
                    break;
                case 1:
                    // Example: Build crypt
                    executePower_BuildCrypt(target);
                    break;
                default:
                    Debug.LogWarning($"IxthusFix: Unknown power index {powerIndex}");
                    break;
            }
        }

        protected virtual void executePower_SpreadFaith(Location target)
        {
            // Placeholder for actual implementation
            Debug.Log($"IxthusFix: Spreading faith at {target.getName()}");
        }

        protected virtual void executePower_BuildCrypt(Location target)
        {
            // Placeholder for actual implementation
            // Would need to instantiate Set_Crypt properly
            Debug.Log($"IxthusFix: Building crypt at {target.getName()}");
        }

        /// <summary>
        /// FIX: Ensure turn tick handles underground locations
        /// </summary>
        public override void turnTick(Map map)
        {
            base.turnTick(map);

            // Process all locations including underground
            foreach (Location loc in map.locations)
            {
                if (loc != null)
                {
                    processLocationTurnTick(loc);
                }
            }
        }

        protected virtual void processLocationTurnTick(Location location)
        {
            // Placeholder for per-location turn processing
            // Add any turn-based effects here
        }
    }
}
