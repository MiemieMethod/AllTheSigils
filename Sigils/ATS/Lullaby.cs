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
        public void AddLullaby()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Lullaby",
                    "When [creature] dies, all opposing creatures gain the asleep sigil.",
                    typeof(Lullaby),
                    GetTexture("lullaby")
                );
            info.SetPixelAbilityIcon(GetTexture("lullaby", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Lullaby.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("lullaby", "");
            }
        }
    }

    public class Lullaby : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            CardModificationInfo mod = new CardModificationInfo();

            mod.abilities = new List<Ability>() { Asleep.ability };
            //i add the ability two times here because otherwise it would just remove it straight away after combat
            if (SigilEffectUtils.combatPhase)
            {
                mod.abilities.Add(Asleep.ability);
            }


            List<CardSlot> opponentSlots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
            foreach (PlayableCard card in opponentSlots.Select(x => x.Card))
            {
                if (card != null)
                {
                    card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.1f);
                    card.AddTemporaryMod(mod);
                }
            }
            yield break;
        }
    }
}