using APIPlugin;
using DiskCardGame;
using HarmonyLib;
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
        private void AddHaste()
        {
            // setup ability
            const string rulebookName = "Haste";
            const string rulebookDescription = "[creature] will attack as soon as it gets played on the board if played not during combat.";
            const string LearnDialogue = "Speed";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Haste);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Haste_a2);
            int powerlevel = 4;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Haste.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Haste), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Haste.ability] = new Tuple<string, string>("void_haste", "");
            }
        }
    }

    public class void_Haste : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.HasAbility(void_Haste.ability);
        }


        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.25f);
            if (!SigilEffectUtils.combatPhase)
            {
                FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.Slot);
            }
            yield return new WaitForSeconds(0.25f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.25f);
            yield break;

        }
    }
}