using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddBlind()
        {
            // setup ability
            const string rulebookName = "Random Strike";
            const string rulebookDescription = "[creature] will strike at opponent slots randomly when it attacks.";
            const string LearnDialogue = "They strike widly...";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_Blind_a2"));
            info.flipYIfOpponent = true;

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Blind");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Blind), tex);

            // set ability to behaviour class
            void_Blind.ability = info.ability;




        }
    }

    public class void_Blind : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }

    [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot", MethodType.Normal)]
    public class SlotAttackSlot_Blind_Patch
    {
        [HarmonyPrefix]
        public static void SlotAttackSlot(ref CardSlot attackingSlot, ref CardSlot opposingSlot, float waitAfter = 0f)
        {
            if (attackingSlot.Card != null && attackingSlot.Card.HasAbility(void_Blind.ability) && !attackingSlot.Card.HasAbility(Ability.AllStrike))
            {
                PlayableCard card = attackingSlot.Card;

                // Get all slots
                List<CardSlot> allSlots = new List<CardSlot>();
                if (card.slot.IsPlayerSlot)
                {
                    allSlots = Singleton<BoardManager>.Instance.opponentSlots;
                }
                else
                {
                    allSlots = Singleton<BoardManager>.Instance.playerSlots;
                }

                CardSlot target = allSlots[Random.Range(0, (allSlots.Count))];

                opposingSlot = target;

            }
        }
    }
}