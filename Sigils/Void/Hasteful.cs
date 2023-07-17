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
        private void AddHasteful()
        {
            // setup ability
            const string rulebookName = "Hasteful";
            const string rulebookDescription = "[creature] will attack as soon as it gets played on the board. It will not attack during normal combat. It will attack at the start of the owner's turn.";
            const string LearnDialogue = "Speed";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Hasteful);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Hasteful_a2);
            int powerlevel = 5;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Hasteful.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Hasteful), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Hasteful.ability] = new Tuple<string, string>("void_Hasteful", "");
            }
        }
    }

    public class void_Hasteful : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.HasAbility(void_Hasteful.ability);
        }


        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.Slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;

        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep && base.Card.HasAbility(void_Hasteful.ability);
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.Slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}