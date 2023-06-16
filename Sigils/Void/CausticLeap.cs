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
        //Request by Tilted hat
        private void AddCaustic()
        {
            // setup ability
            const string rulebookName = "Caustic";
            const string rulebookDescription = "At the end of the towner's turn, [creature] will move in the direction inscribed in the sigil, and drop an acid puddle in their old space.";
            const string LearnDialogue = "What it leaves behind is deadly.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_caustic_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_caustic");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Caustic), tex);

            // set ability to behaviour class
            void_Caustic.ability = info.ability;


        }
    }

    public class void_Caustic : Strafe
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            bool flag = cardSlot.Card == null;
            if (flag)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("void_Acid_Puddle"), cardSlot, 0.1f, true);
            }
            yield break;
        }
    }
}