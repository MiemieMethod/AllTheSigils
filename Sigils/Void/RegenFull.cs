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
        private void AddRegenFull()
        {
            // setup ability
            const string rulebookName = "Regen";
            const string rulebookDescription = "At the end of the owner's turn, [creature] will regen to full health.";
            const string LearnDialogue = "This creature will heal to full Health at the end of it's owner's turn.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 4);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("ability_regen_full_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_regen_full");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_RegenFull), tex);

            // set ability to behaviour class
            void_RegenFull.ability = info.ability;


        }
    }

    public class void_RegenFull : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.HealDamage(base.Card.Status.damageTaken);
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}
