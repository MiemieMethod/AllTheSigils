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
        public void AddAlternating_Scratch()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Alternating Scratch",
                    "When [creature] attacks it will also attack a space adjacent to the attacked slot, the adjacent slot that it will attack will change between left or right at the end of each opponent's turn.",
                    typeof(Alternating_Scratch),
                    GetTexture("alternating_scratch")
                );
            info.SetPixelAbilityIcon(GetTexture("alternating_scratch", true));
            info.powerLevel = 4;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Alternating_Scratch.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("alternating_scratch", "");
            }
        }
    }

    public class Alternating_Scratch : ExtendedAbilityBehaviour
    {
        bool attackingToRight = true;

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public void Start()
        {
            base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("alternating_scratch_right", SaveManager.SaveFile.IsPart2));
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            attackingToRight = !attackingToRight;
            if (attackingToRight)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("alternating_scratch_right", SaveManager.SaveFile.IsPart2));
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("alternating_scratch_left", SaveManager.SaveFile.IsPart2));
            }
            base.Card.RenderCard();
            yield break;
        }

        public override bool RespondsToGetOpposingSlots()
        {
            return true;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> opposingSlots = new List<CardSlot>();
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
            if (attackingToRight)
            {
                if (toRightSlot != null)
                {
                    opposingSlots.Add(toRightSlot.opposingSlot);
                }
                else
                {
                    opposingSlots.Add(base.Card.OpposingSlot());
                }
            }
            else
            {
                if (toLeftSlot != null)
                {
                    opposingSlots.Add(toLeftSlot.opposingSlot);
                }
                else
                {
                    opposingSlots.Add(base.Card.OpposingSlot());
                }
            }
            return opposingSlots;
        }
    }
}