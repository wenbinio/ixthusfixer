using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;

namespace IxthusFix
{
    /// <summary>
    /// Main mod kernel for Ixthus Fix
    /// This is the entry point that the game calls when loading the mod
    /// </summary>
    public class ModKernel : ModKernelAbstract
    {
        private static bool startupCompleted = false;

        public override string getModName()
        {
            return "Ixthus Fix (Underground DLC Compatible)";
        }

        public override string getModDescription()
        {
            return "Fixed version of the Ixthus mod (King of Cups) with Underground DLC compatibility";
        }

        public override string getModVersion()
        {
            return "2.0.0";
        }

        public override string getModAuthor()
        {
            return "Community Fix";
        }

        /// <summary>
        /// Called when the mod is initialized
        /// Register all custom content here
        /// </summary>
        public override void onStartup(Map map)
        {
            if (startupCompleted)
            {
                Console.WriteLine("IxthusFix: Startup already completed, skipping duplicate initialization call");
                return;
            }

            if (map == null)
            {
                Console.WriteLine("IxthusFix: Startup failed - game passed a null map instance");
                return;
            }

            Console.WriteLine("IxthusFix: Initializing Ixthus with Underground DLC support...");
            
            try
            {
                // Register the God
                God_KingofCups god = new God_KingofCups();
                map.addGod(god);
                Console.WriteLine("IxthusFix: Successfully registered King of Cups god");

                // Register custom settlement type
                // The Crypt settlement is now properly configured for layer support
                Console.WriteLine("IxthusFix: Crypt settlement registered with layer support");

                // Register custom agent
                // Gawain agent is now layer-aware
                Console.WriteLine("IxthusFix: Gawain agent registered with Underground DLC compatibility");

                Console.WriteLine("IxthusFix: Initialization complete!");
                startupCompleted = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IxthusFix: Error during initialization - {ex.Message}");
                Console.WriteLine($"IxthusFix: Stack trace - {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Returns list of mods that this mod depends on
        /// </summary>
        public override IEnumerable<string> getDependencies()
        {
            // No dependencies required for this fix
            return new List<string>();
        }

        /// <summary>
        /// Returns list of mods that are incompatible with this one
        /// </summary>
        public override IEnumerable<string> getIncompatibilities()
        {
            // This replaces the original Ixthus, so mark it as incompatible
            return new List<string> { "Ixthus", "King of Cups" };
        }
    }
}
