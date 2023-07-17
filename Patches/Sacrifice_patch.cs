// Using Inscryption
using DiskCardGame;
using HarmonyLib;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace AllTheSigils
{

    [HarmonyPatch(typeof(PlayableCard), "Sacrifice")]
    public class Sacrifice_patch
    {
        public static IEnumerator PatchedSacrifice(PlayableCard __instance)
        {
            AscensionStatsData.TryIncrementStat(AscensionStat.Type.SacrificesMade);
            __instance.Anim.PlaySacrificeSound();
            __instance.Anim.SetSacrificeHoverMarkerShown(false);
            __instance.Anim.SetMarkedForSacrifice(false);
            __instance.Anim.PlaySacrificeParticles();


            List<Ability> abilities = __instance.Info.Abilities.Except(new List<Ability>() { Resourceful.ability }).ToList();
            abilities.RemoveAll(x => __instance.temporaryMods.Any(y => y.negateAbilities.Contains(x)));

            if (abilities.Count == 0)
            {
                abilities = new List<Ability>() { Resourceful.ability };
            }

            Random random = new Random();
            Ability abilityToRemove = abilities[random.Next(abilities.Count)];
            CardModificationInfo mod = new CardModificationInfo() { negateAbilities = new List<Ability>() { abilityToRemove } };
            __instance.AddTemporaryMod(mod);


            if (__instance.TriggerHandler.RespondsToTrigger(Trigger.Sacrifice, Array.Empty<object>()))
            {
                yield return __instance.TriggerHandler.OnTrigger(Trigger.Sacrifice, Array.Empty<object>());
            }
            yield break;
        }

        [HarmonyPrefix]
        public static bool Prefix(PlayableCard __instance, out IEnumerator __result)
        {
            if (__instance.HasAbility(Resourceful.ability) && __instance.Info.Abilities.Count > 0)
            {
                __result = PatchedSacrifice(__instance);
                return false;
            }
            __result = null;
            return true;
        }
    }
}
