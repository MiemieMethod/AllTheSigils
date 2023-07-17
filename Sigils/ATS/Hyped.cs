using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddHyped()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Hyped",
                    "[creature] will attack at the start of the owner's turn in addition to during normal combat.",
                    typeof(Hyped),
                    GetTexture("hyped")
                );
            info.SetPixelAbilityIcon(GetTexture("hyped", true));
            info.powerLevel = 0;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Hyped.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("hyped", "");
            }
        }
    }

    public class Hyped : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.Slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}