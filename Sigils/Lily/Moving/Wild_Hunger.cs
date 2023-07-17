// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddWild_Hunger()
        {
            AbilityInfo info = AbilityManager.New(
                           OldLilyPluginGuid,
                           "Wild hunger",
                           "At the end of its owner's turn, [creature] will move in the direction inscribed on the sigil, however if it hits a card whilst moving, [creature] will stop, the card it hit will perish and [creature] will gain 1+/1+.",
                           typeof(Wild_Hunger),
                           GetTextureLily("wild_hunger")
                       );
            info.SetPixelAbilityIcon(GetTextureLily("wild_hunger", true));
            info.powerLevel = 4;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Wild_Hunger.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("wild_hunger", "");
            }
        }
    }

    public class Wild_Hunger : AbilityBehaviour
    {
        public static CardModificationInfo mod = new CardModificationInfo();

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
            mod.attackAdjustment = 0;
            mod.healthAdjustment = 0;
            if (!base.Card.Info.mods.Contains(mod))
            {
                base.Card.AddTemporaryMod(mod);
            }
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return base.StartCoroutine(this.DoStrafe(toLeft, toRight));
            yield break;
        }

        // Token: 0x06001425 RID: 5157 RVA: 0x0004453A File Offset: 0x0004273A
        protected virtual IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            bool canmoveleft = toLeft != null;
            bool canmoveright = toRight != null;
            if (this.movingLeft && !canmoveleft)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !canmoveright)
            {
                this.movingLeft = true;
            }

            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? canmoveleft : canmoveright;

            if (destination.Card != null)
            {
                yield return destination.Card.Die(false, base.Card);
                mod.attackAdjustment += 1;
                mod.healthAdjustment += 1;
            }
            yield return new WaitForSeconds(0.3f);
            if (destination.Card == null)
            {
                yield return base.StartCoroutine(this.MoveToSlot(destination, destinationValid));
            }
            if (destination != null && destinationValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }

        // Token: 0x06001426 RID: 5158 RVA: 0x00044557 File Offset: 0x00042757
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                CardSlot oldSlot = base.Card.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                yield return this.PostSuccessfulMoveSequence(oldSlot);
                yield return new WaitForSeconds(0.25f);
                oldSlot = null;
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }

        // Token: 0x06001427 RID: 5159 RVA: 0x00044574 File Offset: 0x00042774
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            yield break;
        }

        // Token: 0x04000E13 RID: 3603
        protected bool movingLeft;
    }
}