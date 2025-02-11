using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Rocket.Core.Logging;
using SDG.Unturned;

namespace Zombs_R_Cute_DamageableBarricadeOverride
{
    [HarmonyPatch(typeof(BarricadeManager), nameof(BarricadeManager.damage))]
    public static class BarricadeManagerDamage_Patch
    {
        /// <summary>
        /// This method only runs once from Load()
        /// 
        /// This patches the damage routine to call a custom method check for the id of the
        ///  barricade allowed to be damaged and sets the multiplier back to 1.0
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var newCodes = new List<CodeInstruction>();
            bool found = false;
            for (int i = 0; i < codes.Count(); i++)
            {
                newCodes.Add(codes[i]);

                if (!found &&
                    codes[i + 1].opcode == OpCodes.Ldarg_1 &&
                    codes[i + 2].opcode == OpCodes.Ldarg_2)
                {
                    found = true;
                    // arg 2 is 'times' multiplier
                    newCodes.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    // loc 5 is the 'asset'
                    newCodes.Add(new CodeInstruction(OpCodes.Ldloc, 5));
                    // call the override
                    newCodes.Add(new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(BarricadeManagerDamage_Patch),
                            nameof(AssetDamageMultiplierOverride))));
                    // store the new 'times' multiplier
                    newCodes.Add(new CodeInstruction(OpCodes.Starg, 2));
                }
            }

            foreach (var instruction in newCodes)
            {
                yield return instruction;
            }

            if (found)
                DamageableBarricadeOverridePlugin.Instance.patchSuccess = true;
                
        }

        /// <summary>
        /// Method injected into damage() 
        /// </summary>
        /// <param name="times">times multiplier</param>
        /// <param name="asset">asset being damaged</param>
        /// <returns> times, if asset's id is not found.
        /// 1</returns>
        private static float AssetDamageMultiplierOverride(float times, ItemBarricadeAsset asset)
        {
            if (DamageableBarricadeOverridePlugin.Instance.Configuration.Instance.DamageableBarricades
                .Contains(asset.id))
                return 1;
            return times;
        }
    }
}