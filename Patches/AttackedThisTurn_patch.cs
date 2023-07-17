// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using HarmonyLib;

namespace AllTheSigils
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(PlayableCard), "AttackedThisTurn", MethodType.Getter)]
    public class AttackedThisTurn_patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref PlayableCard __instance, ref bool __result)
        {
            if (__instance.HasAbility(void_Hasteful.ability) || __instance.HasAbility(void_Sluggish.ability))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
