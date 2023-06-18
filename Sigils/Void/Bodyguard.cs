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
        private void AddBodyguard()
        {
            // setup ability
            const string rulebookName = "Bodyguard";
            const string rulebookDescription = "[creature] will redirect the initial attack of a card to it, if the attack was targeting a card in an adjacent space.";
            const string LearnDialogue = "A protector, till the very end.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_BodyGuard);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_BodyGuard_a2);
            int powerlevel = 2;
            bool LeshyUsable = false;
            bool part1Shops = false;
            bool canStack = false;

            // set ability to behaviour class
            void_Bodyguard.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Bodyguard), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Bodyguard.ability] = "void_bodyguard";
            }
        }
    }

    public class void_Bodyguard : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }


    [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot", MethodType.Normal)]
    public class AttackIsBlocked_Bodyguard_Patch
    {
        [HarmonyPrefix]
        public static void SlotAttackSlot(ref CardSlot attackingSlot, ref CardSlot opposingSlot, float waitAfter = 0f)
        {
            if (attackingSlot.Card != null && opposingSlot.Card != null && !attackingSlot.Card.HasAbility(Ability.AllStrike))
            {
                if (opposingSlot.Card != null && !opposingSlot.Card.InOpponentQueue)
                {
                    PlayableCard card = attackingSlot.Card;

                    List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(opposingSlot);

                    if (adjacentSlots.Count > 0 && adjacentSlots[0].Index < opposingSlot.Index)
                    {
                        if (adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                        {
                            if (adjacentSlots[0].Card.Info.HasAbility(void_Bodyguard.ability))
                            {
                                opposingSlot = adjacentSlots[0];
                            }
                        }
                        adjacentSlots.RemoveAt(0);
                    }
                    if (adjacentSlots.Count > 0 && adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                    {
                        if (adjacentSlots[0].Card.Info.HasAbility(void_Bodyguard.ability))
                        {
                            opposingSlot = adjacentSlots[0];
                        }
                    }
                }
            }
        }
    }
}