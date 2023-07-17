using APIPlugin;
using DiskCardGame;
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
        //Request by Blind
        private void AddSluggish()
        {
            // setup ability
            const string rulebookName = "Sluggish";
            const string rulebookDescription = "[creature] will not attack during normal combat. It will instead attack after the Opponent's cards attack.";
            const string LearnDialogue = "A bit slow there eh?";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Sluggish);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Sluggish_a2);
            int powerlevel = -1;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Sluggish.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Sluggish), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Sluggish.ability] = new Tuple<string, string>("void_Sluggish", "");
            }
        }
    }

    public class void_Sluggish : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard == playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}