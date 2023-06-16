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
        private void AddPathetic()
        {
            // setup ability
            const string rulebookName = "Pathetic Sacrifice";
            const string rulebookDescription = "[creature] is so pathetic, it is not a worthy or noble sacrifice. A card with this sigil is meant to stay on the board, and thus can't be targeted by the hammer.";
            const string LearnDialogue = "That is not a noble, or worthy sacrifice";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -3);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_Pathetic_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_pathetic");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Pathetic), tex);

            // set ability to behaviour class
            void_Pathetic.ability = info.ability;


        }
    }

    public class void_Pathetic : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }

    [HarmonyPatch(typeof(HammerItem), "GetValidTargets", MethodType.Normal)]
    public class PatheticSacrifice_Hammer_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref List<CardSlot> __result)
        {
            if (Plugin.configHammerBlock.Value)
            {
                List<CardSlot> playerSlotsCopy = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
                playerSlotsCopy.RemoveAll((CardSlot x) => x.Card == null);
                playerSlotsCopy.RemoveAll((CardSlot x) => x.Card.HasAbility(void_Pathetic.ability));
                __result = playerSlotsCopy;
                return false;
            }
            return true;
        }
    }



    [HarmonyPatch(typeof(CardInfo), "Sacrificable", MethodType.Getter)]
    public class CardInfo_Sacrificable
    {
        [HarmonyPostfix]
        public static void Postfix(ref CardInfo __instance, ref bool __result)
        {
            if (__instance.abilities.Contains(void_Pathetic.ability) || __instance.ModAbilities.Contains(void_Pathetic.ability))
            {
                __result = false;
            }
        }
    }
}