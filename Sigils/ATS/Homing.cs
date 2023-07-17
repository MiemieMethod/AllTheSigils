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
using Random = System.Random;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddHoming()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Homing",
                    "If the opposing slot is empty, and there is a creature in a neighbouring lane, [creature] will attack that creature instead of attacking the opposing slot.",
                    typeof(Homing),
                    GetTexture("homing")
                );
            info.SetPixelAbilityIcon(GetTexture("homing", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Homing.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("homing", "");
            }
        }
    }

    public class Homing : ExtendedAbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RemoveDefaultAttackSlot()
        {
            return base.Card.OpposingCard() == null;
        }

        public override bool RespondsToGetOpposingSlots()
        {
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
            return !(base.Card.OpposingCard() == null && toLeftSlot?.opposingSlot?.Card == null && toRightSlot?.opposingSlot?.Card == null);
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> opposingSlots = new List<CardSlot>();
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);

            if (base.Card.OpposingCard() == null)
            {
                List<CardSlot> slots = new List<CardSlot>() { toLeftSlot.opposingSlot, toRightSlot.opposingSlot };
                slots = slots.Where(x => x?.Card != null).ToList();

                Random random = new Random();
                opposingSlots.Add(slots[random.Next(slots.Count)]);
            }
            return opposingSlots;
        }
    }
}