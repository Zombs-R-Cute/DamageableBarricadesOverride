using HarmonyLib;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Zombs_R_Cute_DamageableBarricadeOverride
{
    [HarmonyPatch]
    public class DamageableBarricadeOverridePlugin : RocketPlugin<DamageableBarricadeOverrideConfiguration>
    {
        public static DamageableBarricadeOverridePlugin Instance;
    
        protected override void Load()
        {
            Instance = this;
        
            Harmony harmony = new Harmony("DamagableBarricadeOverride");
            Harmony.DEBUG = true;
            harmony.PatchAll();
            
            Logger.LogWarning("DamageableBarricadeOverridePlugin - The following IDs are damagable:");
            foreach (var barricade in Configuration.Instance.DamageableBarricades)
            {
                Logger.LogWarning(barricade.ToString());
            }
        }
    }
}