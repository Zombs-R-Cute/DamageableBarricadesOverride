using HarmonyLib;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Zombs_R_Cute_DamageableBarricadeOverride
{
    [HarmonyPatch]
    public class DamageableBarricadeOverridePlugin : RocketPlugin<DamageableBarricadeOverrideConfiguration>
    {
        public static DamageableBarricadeOverridePlugin Instance;
        public bool patchSuccess;
        
        protected override void Load()
        {
            Instance = this;
            Harmony harmony = new Harmony("DamagableBarricadeOverride");
            harmony.PatchAll();
            
            if(!patchSuccess)
            {
                Logger.LogError("Patch failed!");
                return;
            }   
            
            Logger.Log("BarricadeManagerDamage_Patch: Patch applied");
            Logger.LogWarning("DamageableBarricadeOverridePlugin - The following IDs are damagable:");
            foreach (var barricade in Configuration.Instance.DamageableBarricades)
            {
                Logger.LogWarning(barricade.ToString());
            }
        }
    }
}