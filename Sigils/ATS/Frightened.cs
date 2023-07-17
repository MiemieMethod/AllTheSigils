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
        public void AddFrightened()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Frightened",
                    "When a creature moves into the space opposing [creature], [creature] will move away from that card to a random free adjacent spot, while prioritizing slots without any opposing creatures.",
                    typeof(Frightened),
                    GetTexture("frightened")
                );
            info.SetPixelAbilityIcon(GetTexture("frightened", true));
            info.powerLevel = -1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Frightened.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("frightened", "");
            }
        }
    }

    public class Frightened : AbilityBehaviour
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

            List<CardSlot> moveableToCardSlots = new List<CardSlot>() { toLeftSlot, toRightSlot };
            moveableToCardSlots = moveableToCardSlots.Where(x => x != null && x.Card == null).ToList();
            if (moveableToCardSlots.Count == 0)
            {
                yield break;
            }

            List<CardSlot> cardSlotsWithPriority = moveableToCardSlots.Where(x => x.opposingSlot.Card == null).ToList();
            Random random = new Random();
            if (cardSlotsWithPriority.Count > 0)
            {
                yield return SigilEffectUtils.moveCard(base.Card, cardSlotsWithPriority[random.Next(cardSlotsWithPriority.Count)]);
            }
            else
            {
                yield return SigilEffectUtils.moveCard(base.Card, moveableToCardSlots[random.Next(moveableToCardSlots.Count)]);
            }
            yield break;
        }
    }
}