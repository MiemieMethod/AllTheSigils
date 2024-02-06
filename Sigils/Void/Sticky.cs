using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //NEED TO FIX, ALSO MAYBE RENAME TO TRACKER OR STALKER?


        //Request by Blind
        private void AddSticky()
        {
            // setup ability
            const string rulebookName = "Sticky";
            const string rulebookDescription = "If the card opposing [creature] moves, [creature] will try to move with it";
            const string LearnDialogue = "The trail they leave behind, hurts.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Sticky);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Sticky_a2);
            int powerlevel = 2;
            bool LeshyUsable = Plugin.configAcidTrail.Value;
            bool part1Shops = true;
            bool canStack = false;



            // set ability to behaviour class
            void_Sticky.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Sticky), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Sticky.ability] = new Tuple<string, string>("void_Sticky", "");
            }
        }
    }

    public class void_Sticky : AbilityBehaviour, IOnCardAssignedToSlotContext
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public bool RespondsToCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            return base.Card.OnBoard && oldSlot == base.Card.OpposingSlot() && base.Card.OpposingCard() == null;
        }

        public IEnumerator OnCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            List<CardSlot> AllSlots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy;
            CardSlot baseCardNewSlot = AllSlots.Where(x => x.Index == newSlot.Index).First();

            if (baseCardNewSlot.Card == null)
            {
                yield return SigilEffectUtils.moveCard(base.Card, baseCardNewSlot);
            }
            yield break;
        }
    }
}