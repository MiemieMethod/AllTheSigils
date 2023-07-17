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
        public void AddInaccurate()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Inaccurate",
                    "[creature] will strike a random one of the opposing spaces to the left and right of the spaces across from it as well as the space in front of it.",
                    typeof(Inaccurate),
                    GetTexture("inaccurate")
                );
            info.SetPixelAbilityIcon(GetTexture("inaccurate", true));
            info.powerLevel = 0;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Inaccurate.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("inaccurate", "");
            }
        }
    }

    public class Inaccurate : ExtendedAbilityBehaviour
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
            return true;
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
            List<CardSlot> slots = new List<CardSlot>() { toLeftSlot.opposingSlot, base.Card.Slot.opposingSlot, toRightSlot.opposingSlot };
            slots = slots.Where(x => x != null).ToList();

            Random random = new Random();
            opposingSlots.Add(slots[random.Next(slots.Count)]);

            return opposingSlots;
        }
    }
}