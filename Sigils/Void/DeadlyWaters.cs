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
        //Original
        private void AddDeadlyWaters()
        {
            // setup ability
            const string rulebookName = "Deadly Waters";
            const string rulebookDescription = "[creature] will kill cards that attacked over it while it was face-down. Does not affect cards that have Airborne or Made of Stone.";
            const string LearnDialogue = "It's not always safe to go into the waters.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_DeadlyWaters);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_DeadlyWaters_a2);
            int powerlevel = 4;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_DeadlyWaters.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_DeadlyWaters), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_DeadlyWaters.ability] = new Tuple<string, string>("void_DeadlyWaters", "");
            }
        }
    }

    public class void_DeadlyWaters : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public static List<PlayableCard> cardsThatHitBaseCard = new List<PlayableCard>();

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return true;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (playerTurnEnd == base.Card.OpponentCard)
            {
                for (int i = 0; i < cardsThatHitBaseCard.Count; i++)
                {
                    yield return cardsThatHitBaseCard[i].Die(false, base.Card);
                    cardsThatHitBaseCard.Remove(cardsThatHitBaseCard[i]);
                }
            }
            yield break;
        }

        [HarmonyPatch(typeof(CombatPhaseManager), nameof(CombatPhaseManager.SlotAttackSlot))]
        public class DeadlyWatersPatch
        {
            [HarmonyPostfix]
            public static void Postfix(ref CardSlot attackingSlot, ref CardSlot opposingSlot)
            {
                if (opposingSlot?.Card != null)
                {
                    if (opposingSlot.Card.HasAbility(void_DeadlyWaters.ability) && opposingSlot.Card.FaceDown)
                    {
                        cardsThatHitBaseCard.Add(attackingSlot.Card);
                    }
                }
            }
        }
    }
}