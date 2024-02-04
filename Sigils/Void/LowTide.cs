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
        //Original
        private void AddLowTide()
        {
            // setup ability
            const string rulebookName = "Low Tide";
            const string rulebookDescription = "While [creature] is on the board, it will negate the waterborne sigil of cards that are played after it on it's side of the board.";
            const string LearnDialogue = "The waters rise.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_LowTide);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_LowTide_a2);
            int powerlevel = 1;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_LowTide.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_LowTide), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_LowTide.ability] = new Tuple<string, string>("void_LowTide", "");
            }
        }
    }

    public class void_LowTide : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            //Make sure our card is on board 
            // && that the other card isnt our card (for some reason RespondsToOtherCardResolve also resolves this card
            // && finally that the othercard has submerge or squid submerge
            return base.Card.OnBoard && otherCard.slot != base.Card.slot && (otherCard.HasAbility(Ability.Submerge) || otherCard.HasAbility(Ability.SubmergeSquid));
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            /// I hate how I coded this but I couldn't figure out (might be cause I made this at 5am) how to make sure the base card is on the players side
            /// and the card that is resolving on board is also on the player's side. So I just got all slots based on if the base.card.slot is a player slot or not
            /// then ran a for loop to check if the other card is in a slot on that side. 

            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.25f);
            List<CardSlot> cardSlots = Singleton<BoardManager>.Instance.GetSlots(base.Card.slot.IsPlayerSlot);
            for (var index = 0; index < cardSlots.Count; index++)
            {
                if (cardSlots[index].Card == otherCard)
                {
                    if (otherCard.HasAbility(Ability.Submerge) || otherCard.HasAbility(Ability.SubmergeSquid))
                    {
                        yield return new WaitForSeconds(0.3f);
                        CardModificationInfo negateSubmergeMod = new CardModificationInfo();
                        negateSubmergeMod.singletonId = "void_negate_abilities";
                        negateSubmergeMod.negateAbilities.Add(Ability.Submerge);
                        negateSubmergeMod.negateAbilities.Add(Ability.SubmergeSquid);
                        CardInfo OpponentCardInfo = otherCard.Info.Clone() as CardInfo;
                        OpponentCardInfo.Mods.Add(negateSubmergeMod);
                        OpponentCardInfo.Mods.AddRange(otherCard.Info.Mods);
                        otherCard.SetInfo(OpponentCardInfo);
                        otherCard.Anim.PlayTransformAnimation();
                        yield return new WaitForSeconds(0.3f);
                        yield return base.LearnAbility(0.25f);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
            yield break;
        }
    }
}