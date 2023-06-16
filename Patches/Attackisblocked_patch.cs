// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using HarmonyLib;

namespace AllTheSigils
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(PlayableCard), "AttackIsBlocked", 0)]
    public class Attackisblocked_patch
    {
        // Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
        [HarmonyPostfix]
        public static void Postfix(CardSlot opposingSlot, bool __result, ref PlayableCard __instance)
        {
            if (opposingSlot.Card != null)
            {
                if (opposingSlot.Card.Info.HasAbility(Ability.Flying) && __instance.Info.HasAbility(Short.ability))
                {
                    __result = true;
                }
            }
        }
    }
}
