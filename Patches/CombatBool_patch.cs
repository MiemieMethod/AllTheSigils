using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AllTheSigils
{
    public class CombatBool_patch
    {
        [HarmonyPatch(typeof(CombatPhaseManager), "DoCombatPhase", MethodType.Normal)]
        public class Combatphase_Start_patch
        {
            [HarmonyPrefix]
            public static void DoCombatPhase()
            {
                SigilEffectUtils.combatPhase = true;
            }
        }

        [HarmonyPatch(typeof(TurnManager), "DoUpkeepPhase", MethodType.Normal)]
        public class Combatphase_End_patch
        {
            [HarmonyPrefix]
            public static void DoUpkeepPhase()
            {
                SigilEffectUtils.combatPhase = false;
            }
        }
    }
}
