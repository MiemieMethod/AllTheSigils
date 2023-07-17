using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;


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
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Fisher);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Fisher_a2);
            int powerlevel = 1;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Fisher.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Fisher), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Fisher.ability] = new Tuple<string, string>("void_fisher", "");
            }
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
            yield return new WaitForSeconds(0.3f);
            slot.Card.SetFaceDown(false, false);
            slot.Card.UpdateFaceUpOnBoardEffects();
            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.2f);
            yield break;
        }
    }
}