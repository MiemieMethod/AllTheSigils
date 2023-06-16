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
        private void AddDesperation()
        {
            // setup ability
            const string rulebookName = "Desperation";
            const string rulebookDescription = "[creature] is damaged to 1 health, it will gain 3 power.";
            const string LearnDialogue = "So close to death, it strikes out.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 0);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_desperation");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_desperation), tex);

            // set ability to behaviour class
            void_desperation.ability = info.ability;


        }
    }

    public class void_desperation : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        private CardModificationInfo mod;

        private void Start()
        {
            this.mod = new CardModificationInfo();
            this.mod.attackAdjustment = 3;
        }

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return base.Card.Health == 1;
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            base.Card.temporaryMods.Add(this.mod);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}