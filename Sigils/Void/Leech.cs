using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Port from Cyn Sigil a day
        private void AddLeech()
        {
            // setup ability
            const string rulebookName = "Leech";
            const string rulebookDescription = "When [creature] deals damage, it will heal 1 Health for each damage dealt to a card.";
            const string LearnDialogue = "Vigor from blood!";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3, Plugin.configLeech.Value);
            info.canStack = true;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("ability_leech_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_leech");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Leech), tex);

            // set ability to behaviour class
            void_Leech.ability = info.ability;


        }
    }

    public class void_Leech : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return amount > 0;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (base.Card.Status.damageTaken > 0)
            {
                base.Card.HealDamage(Mathf.Clamp(amount, 1, base.Card.Status.damageTaken));
            }
            base.Card.OnStatsChanged();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.25f);
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }

}
