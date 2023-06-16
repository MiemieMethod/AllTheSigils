using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Oiriginal cause FUCK SUBMERG
        private void AddFisher()
        {
            // setup ability
            const string rulebookName = "Lure";
            const string rulebookDescription = "[creature] will cause facedown cards to become face up when attacking.";
            const string LearnDialogue = "The Submerge are brought back to the surface";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 1);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_fisher_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_fisher");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Fisher), tex);

            // set ability to behaviour class
            void_Fisher.ability = info.ability;


        }
    }



    public class void_Fisher : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (slot.Card != null)
            {
                return attacker == this.Card && slot.Card.FaceDown;
            }
            return false;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.25f);
            slot.Card.SetFaceDown(false, false);
            slot.Card.UpdateFaceUpOnBoardEffects();
            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}