// Using Inscryption
using DiskCardGame;
using HarmonyLib;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; // Loading Sigil and Card art.
using System.Linq;
using UnityEngine;

namespace AllTheSigils
{
    // Token: 0x02000006 RID: 6Picky
    [HarmonyPatch(typeof(Brittle), "OnAttackEnded")]
    public class Brittle_patch
    {
        // Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
        [HarmonyPrefix]
        public static bool Prefix()
        {
            foreach (List<Ability> abilities in Singleton<BoardManager>.Instance.AllSlotsCopy.Select(x => x.Card?.Info?.Abilities ?? new List<Ability>()))
            {
                if (abilities.Contains(Puppets_Gift.ability))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
