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
        //Request by Tilted hat
        private void AddCaustic()
        {
            // setup ability
            const string rulebookName = "Caustic";
            const string rulebookDescription = "At the end of the towner's turn, [creature] will move in the direction inscribed in the sigil, and drop an acid puddle in their old space.";
            const string LearnDialogue = "What it leaves behind is deadly.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Caustic);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Caustic_a2);
            int powerlevel = 2;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;



            // set ability to behaviour class
            void_Caustic.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Caustic), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Caustic.ability] = new Tuple<string, string>("void_caustic", "");
            }
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
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.25f);
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("void_Acid_Puddle"), cardSlot, 0.1f, true);
                yield return new WaitForSeconds(0.25f);
            }
            yield break;
        }
    }
}