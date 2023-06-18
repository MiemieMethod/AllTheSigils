// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddWarper()
        {
            AbilityInfo info = AbilityManager.New(
                         OldLilyPluginGuid,
                         "Warper",
                         "At the end of its owner's turn, [creature] will move to the right, jumping over any creatures in its path, if it encounters the edge of the board, it will loop over to the other side.",
                         typeof(Warper),
                         GetTexture("warper")
                     );
            info.SetPixelAbilityIcon(GetTexture("warper", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Warper.ability = info.ability;
        }
    }

    public class Warper : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
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
            if (toLeft == null) { toLeft = Singleton<BoardManager>.Instance.playerSlots.Last(); }
            if (toRight == null) { toRight = Singleton<BoardManager>.Instance.playerSlots.First(); }

            CardSlot toLefttwice = Singleton<BoardManager>.Instance.GetAdjacent(toLeft, true);
            CardSlot toRighttwice = Singleton<BoardManager>.Instance.GetAdjacent(toRight, false);
            if (toLefttwice == null) { toLefttwice = Singleton<BoardManager>.Instance.playerSlots.Last(); }
            if (toRighttwice == null) { toRighttwice = Singleton<BoardManager>.Instance.playerSlots.First(); }

            bool canmoveleft = (toLeft.Card == null || toLefttwice.Card == null);
            bool canmoveright = (toRight.Card == null || toRighttwice.Card == null);
            if (this.movingLeft && !canmoveleft)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !canmoveright)
            {
                this.movingLeft = true;
            }
            CardSlot destination = this.movingLeft ? toLeft : toRight;
            if (destination.Card != null)
            {
                destination = this.movingLeft ? toLefttwice : toRighttwice;
            }
            Plugin.Log.LogInfo(destination.Index);
            yield return base.StartCoroutine(this.MoveToSlot(destination));
            yield break;
        }

        // Token: 0x06001426 RID: 5158 RVA: 0x00044557 File Offset: 0x00042757
        protected IEnumerator MoveToSlot(CardSlot destination)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destination.Card == null)
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