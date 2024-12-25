using System.Collections.Generic;
using System.Xml.Serialization;
using HarmonyLib;
using Rocket.API;

namespace Zombs_R_Cute_DamageableBarricadeOverride
{
    public class DamageableBarricadeOverrideConfiguration : IRocketPluginConfiguration
    {
        [XmlArray("DamagableIds"), XmlArrayItem("ID")]
        public HashSet<ushort> DamageableBarricades;

        public void LoadDefaults()
        {
            DamageableBarricades = new HashSet<ushort>() { 51692 };// 51692 Escalation's D.A.V.E.
        }
    }
}