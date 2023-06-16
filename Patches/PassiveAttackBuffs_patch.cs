// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using System.Collections.Generic;
// Modding Inscryption
using System.Linq;

namespace AllTheSigils
{

    [HarmonyPatch(typeof(PlayableCard), "GetPassiveAttackBuffs")]
    public class PassiveAttackBuffs_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, ref PlayableCard __instance)
        {
            if (__instance.OnBoard)
            {
                List<CardSlot> Slots = __instance.OpponentCard ? Singleton<BoardManager>.Instance.opponentSlots : Singleton<BoardManager>.Instance.playerSlots;
                foreach (CardSlot slot in Slots)
                {
                    if (slot.Card != null && slot.Card.Info.HasAbility(Tribe_Attack.ability))
                    {
                        bool hassametribes = slot.Card.Info.tribes.Any(__instance.Info.tribes.Contains);
                        if (slot.Card != __instance && hassametribes)
                        {
                            __result += 1;
                        }
                    }

                    if (slot.Card != null && slot.Card.Info.HasAbility(Linguist.ability))
                    {
                        bool IsTalkingCard = __instance.Info.specialAbilities.Contains(SpecialTriggeredAbility.TalkingCardChooser);
                        if (IsTalkingCard)
                        {
                            __result += 1;
                        }
                    }
                }


                if (__instance.HasAbility(Bond.ability))
                {
                    CardSlot LeftSlot = Singleton<BoardManager>.Instance.GetAdjacentSlots(__instance.slot)[0];
                    if (LeftSlot?.Card != null)
                    {
                        __result += 1;
                    }
                }
            }
        }
    }
}