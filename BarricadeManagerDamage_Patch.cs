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
        /// This patches the damage routine to check for the id of the barricade allowed to be damaged and sets the multiplier back to 1.0
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
                    newCodes.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    newCodes.Add(new CodeInstruction(OpCodes.Ldloc, 5));
                    newCodes.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BarricadeManagerDamage_Patch), nameof(AssetDamageMultiplierOverride))));
                    newCodes.Add(new CodeInstruction(OpCodes.Starg, 2));
                }
            }
            foreach (var instruction in newCodes)
            {
                yield return instruction;
            }
            Logger.Log("BarricadeManagerDamage_Patch: Patch applied");
        }

        private static float AssetDamageMultiplierOverride(float times, ItemBarricadeAsset asset)
        {
            if (DamageableBarricadeOverridePlugin.Instance.Configuration.Instance.DamageableBarricades.Contains(asset.id)) 
                return 1;
            return times;
        }
    }
}