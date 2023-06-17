using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddManeuver()
        {
            // setup ability
            const string rulebookName = "Maneuver";
            const string rulebookDescription = "At the start of the owner's turn, [creature] will strafe in the direction inscribed on the sigil if there is a creature in the opposing slot from it. Else it will strafe in the opposite direction inscribed on the sigil.";
            const string LearnDialogue = "It runs";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Maneuver);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Maneuver_a2);
            int powerlevel = 2;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Maneuver.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Maneuver), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
        }
    }

    public class void_Maneuver : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card != null && base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            if (base.Card.slot.opposingSlot.Card != null)
            {
                yield return this.DoStrafe(toLeft, toRight);
            }
            else
            {
                yield return this.DoStrafe(toRight, toLeft);
            }
            yield break;
        }

        protected virtual IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            bool flag = this.movingLeft && !toLeftValid;
            if (flag)
            {
                this.movingLeft = false;
            }
            bool flag2 = !this.movingLeft && !toRightValid;
            if (flag2)
            {
                this.movingLeft = true;
            }
            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? toLeftValid : toRightValid;
            yield return this.MoveToSlot(destination, destinationValid);
            bool flag3 = destination != null && destinationValid;
            if (flag3)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }

        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            bool flag = destination != null && destinationValid;
            if (flag)
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

        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            bool flag = base.Card.Info.name == "Snelk";
            if (flag)
            {
                bool flag2 = oldSlot.Card == null;
                if (flag2)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("Snelk_Neck"), oldSlot, 0.1f, true);
                }
            }
            yield break;
        }

        protected bool movingLeft;
    }
}