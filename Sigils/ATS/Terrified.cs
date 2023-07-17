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
        public void AddTerrified()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Terrified",
                    "When a creature moves into the space opposing [creature], [creature] will move away from that card to a random free adjacent spot while pushing any creatures in it's way, while prioritizing slots without any opposing creatures.",
                    typeof(Terrified),
                    GetTexture("terrified")
                );
            info.SetPixelAbilityIcon(GetTexture("terrified", true));
            info.powerLevel = 0;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Terrified.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("terrified", "");
            }
        }
    }

    public class Terrified : StrafePush
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override int Priority
        {
            get
            {
                return -1;
            }
        }

        //these overrides are just here so that i can inherit StrafePush and use it's other methods
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return false;
        }

        public override IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            yield break;
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return this.RespondsToTrigger(otherCard);
        }

        // Token: 0x06001579 RID: 5497 RVA: 0x000497A8 File Offset: 0x000479A8
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return MoveAway();
            yield break;
        }

        // Token: 0x0600157A RID: 5498 RVA: 0x0004979F File Offset: 0x0004799F
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return this.RespondsToTrigger(otherCard);
        }

        // Token: 0x0600157B RID: 5499 RVA: 0x000497BE File Offset: 0x000479BE
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            yield return MoveAway();
            yield break;
        }

        // Token: 0x0600157C RID: 5500 RVA: 0x000497D4 File Offset: 0x000479D4
        private bool RespondsToTrigger(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
        }

        public IEnumerator MoveAway()
        {
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);

            List<CardSlot> moveableToCardSlots = new List<CardSlot>() { };
            if (toLeftSlot != null && (toLeftSlot.Card == null || SlotHasSpace(toLeftSlot, true)))
            {
                moveableToCardSlots.Add(toLeftSlot);
            }
            if (toRightSlot != null && (toRightSlot.Card == null || SlotHasSpace(toRightSlot, false)))
            {
                moveableToCardSlots.Add(toRightSlot);
            }
            Plugin.Log.LogInfo(moveableToCardSlots.Count);

            if (moveableToCardSlots.Count == 0)
            {
                yield break;
            }

            List<CardSlot> cardSlotsWithPriority = moveableToCardSlots.Where(x => x.opposingSlot.Card == null).ToList();
            Random random = new Random();
            if (cardSlotsWithPriority.Count > 0)
            {
                CardSlot slotToMoveTo = cardSlotsWithPriority[random.Next(cardSlotsWithPriority.Count)];
                if (slotToMoveTo.Card != null)
                {
                    yield return RecursivePush(slotToMoveTo, slotToMoveTo == toLeftSlot, null);
                }
                yield return SigilEffectUtils.moveCard(base.Card, cardSlotsWithPriority[random.Next(cardSlotsWithPriority.Count)]);
            }
            else
            {
                CardSlot slotToMoveTo = moveableToCardSlots[random.Next(moveableToCardSlots.Count)];
                if (slotToMoveTo.Card != null)
                {
                    yield return RecursivePush(slotToMoveTo, slotToMoveTo == toLeftSlot, null);
                }
                yield return SigilEffectUtils.moveCard(base.Card, moveableToCardSlots[random.Next(moveableToCardSlots.Count)]);
            }
            yield break;
        }
    }
}