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
        private void AddPrideful()
        {
            // setup ability
            const string rulebookName = "Prideful";
            const string rulebookDescription = "[creature] will not attack a card with a power 2 lower than its own.";
            const string LearnDialogue = "A creature's pride will be it's downfall.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1, Plugin.configPrideful.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_Prideful_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Prideful");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Prideful), tex);

            // set ability to behaviour class
            void_Prideful.ability = info.ability;


        }
    }

    public class void_Prideful : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;
    }

    [HarmonyPatch(typeof(PlayableCard), "AttackIsBlocked", MethodType.Normal)]
    public class AttackIsBlocked_Prideful_Patch
    {

        [HarmonyPostfix]
        public static void Postfix(ref CardSlot opposingSlot, ref bool __result, PlayableCard __instance)
        {
            if (__instance.OnBoard && opposingSlot.Card != null && __instance.HasAbility(void_Prideful.ability))
            {
                int cardAttack = __instance.Attack - 2;
                int opposingAttack = opposingSlot.Card.Attack;
                if (cardAttack > opposingAttack)
                {
                    __result = true;
                }
            }
        }
    }
}