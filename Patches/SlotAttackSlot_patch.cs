using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AllTheSigils.Patches
{
    [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot", 0)]
    public class SlotAttackSlot_patch
    {
        [HarmonyPrefix]
        public static bool SlotAttackSlotPrefix(ref CardSlot attackingSlot, ref CardSlot opposingSlot, float waitAfter = 0f)
        {
            if (opposingSlot.Card != null && attackingSlot.Card != null)
            {
                if (attackingSlot.Card.HasAbility(Sympathetic.ability) && opposingSlot.Card.Health < attackingSlot.Card.Health)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
