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
        public void AddChoreography()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Choreography",
                    "On activation [creature] will cycle through: don't move, move left and move right. At the end of the owner's turn [creature] will move in the chosen way.",
                    typeof(Choreography),
                    GetTexture("choreo_right")
                );
            info.SetPixelAbilityIcon(GetTexture("choreo_right", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;
            info.activated = true;

            Choreography.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("choreo_right", "");
            }
        }
    }

    public class Choreography : ActivatedAbilityBehaviour
    {
        //0 = no move 
        //1 = move right
        //2 = move left
        public int movementType = 1;

        public bool movingLeft;

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override int BonesCost
        {
            get
            {
                return 0;
            }
        }


        public override IEnumerator Activate()
        {
            movementType++;
            if (movementType == 3)
            {
                movementType = 0;
            }

            if (movementType == 0)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("choreo_stop", SaveManager.SaveFile.IsPart2));
            }
            if (movementType == 1)
            {
                movingLeft = false;
                base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("choreo_right", SaveManager.SaveFile.IsPart2));
            }
            if (movementType == 2)
            {
                movingLeft = true;
                base.Card.RenderInfo.OverrideAbilityIcon(Ability, Plugin.GetTexture("choreo_left", SaveManager.SaveFile.IsPart2));
            }
            base.Card.RenderCard();
            yield break;
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.OpponentCard != playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (movementType != 0)
            {
                CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
                CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.25f);
                yield return DoStrafe(toLeft, toRight);
            }
            yield break;
        }

        public IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            bool flag = toLeft != null && toLeft.Card == null;
            bool flag2 = toRight != null && toRight.Card == null;
            if (this.movingLeft && !flag)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !flag2)
            {
                this.movingLeft = true;
            }
            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? flag : flag2;
            yield return this.MoveToSlot(destination, destinationValid);
            if (destination != null && destinationValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        public IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                CardSlot oldSlot = base.Card.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
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
    }
}