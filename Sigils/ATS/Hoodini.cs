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
        public void AddHoodini()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Hoodini",
                    "When [creature] is played, it will swap places with the opposing creature.",
                    typeof(Hoodini),
                    GetTexture("hoodini")
                );
            info.SetPixelAbilityIcon(GetTexture("hoodini", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Hoodini.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("hoodini", "");
            }
        }
    }

    public class Hoodini : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            PlayableCard opposingCard = base.Card.OpposingCard();
            if (opposingCard != null)
            {
                CardSlot oldOpposingCardSlot = opposingCard.Slot;
                CardSlot oldBaseCardSlot = base.Card.Slot;
                opposingCard.UnassignFromSlot();

                yield return SigilEffectUtils.moveCard(base.Card, oldOpposingCardSlot);
                yield return SigilEffectUtils.moveCard(opposingCard, oldBaseCardSlot);
                opposingCard.Anim.SetCardRendererFlipped(false);
                opposingCard.RenderCard();
            }
            yield break;
        }
    }
}