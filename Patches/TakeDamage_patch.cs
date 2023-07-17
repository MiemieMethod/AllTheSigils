using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllTheSigils.Patches
{
    [HarmonyPatch(typeof(PlayableCard), "TakeDamage")]
    public class TakeDamage_patch
    {
        [HarmonyPrefix]
        private static void Prefix(ref PlayableCard __instance, ref int damage)
        {
            if (__instance.HasAbility(Armoured.ability))
            {
                damage = (int)Math.Floor((double)(damage / 2));
            }
        }
    }
}
