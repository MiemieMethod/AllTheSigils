// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using HarmonyLib;

namespace AllTheSigils
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(PlayableCard), "CanAttackDirectly", 0)]
    public class CanAttackDirectly_patch
    {
        [HarmonyPostfix]
        public static void Postfix(CardSlot opposingSlot, ref bool __result, ref PlayableCard __instance)
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
