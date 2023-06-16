using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddCoward()
        {
            // setup ability
            const string rulebookName = "Cowardly";
            const string rulebookDescription = "[creature] will not attack a card with a power 2 higher than its own.";
            const string LearnDialogue = "It would rather flee than fight";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1, Plugin.configCowardly.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_coward_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Coward");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Coward), tex);

            // set ability to behaviour class
            void_Coward.ability = info.ability;




        }
    }

    public class void_Coward : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;
    }

    [HarmonyPatch(typeof(PlayableCard), "AttackIsBlocked", MethodType.Normal)]
    public class AttackIsBlocked_Cowardly_Patch
    {

        [HarmonyPostfix]
        public static void Postfix(ref CardSlot opposingSlot, ref bool __result, PlayableCard __instance)
        {
            if (__instance.OnBoard && opposingSlot.Card != null && __instance.HasAbility(void_Coward.ability))
            {
                int cardAttack = __instance.Attack;
                int opposingAttack = opposingSlot.Card.Attack - 2;

                if (cardAttack < opposingAttack)
                {
                    __result = true;

                }
            }
        }
    }
}