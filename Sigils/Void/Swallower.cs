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
        private void AddSwallower()
        {
            // setup ability
            const string rulebookName = "Swallower";
            const string rulebookDescription = "After [creature] attacks, it will destroy the card it attacks if it has less health than it and heal to full health.";
            const string LearnDialogue = "Gone in one gulp.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_swallower);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_swallower_a2);
            int powerlevel = 3;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Swallower.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Swallower), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_StrongWind.ability] = new Tuple<string, string>("void_Swallower", "");
            }
        }
    }

    public class void_Swallower : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return amount > 0 && target != null && !target.Dead && (target.Health < base.Card.Health) && !target.HasAbility(Ability.MadeOfStone);
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);
            target.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.15f);
            yield return target.Die(false, base.Card, true);
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.15f);
            base.Card.HealDamage(base.Card.Status.damageTaken);
            yield return new WaitForSeconds(0.15f);
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}