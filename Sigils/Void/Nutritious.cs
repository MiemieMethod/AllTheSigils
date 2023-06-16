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
        private void AddNutritious()
        {
            // setup ability
            const string rulebookName = "Nutritious";
            const string rulebookDescription = "When [creature] is sacrificed, it adds 1 power and 2 health to the card it was sacrificed for.";
            const string LearnDialogue = "That creature is so full of nutrients, the creature you play comes in stronger!";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_nutritious");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Nutritious), tex);

            // set ability to behaviour class
            void_Nutritious.ability = info.ability;


        }
    }

    public class void_Nutritious : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        private void Start()
        {
            this.mod = new CardModificationInfo();
            this.mod.healthAdjustment = 2;
            this.mod.attackAdjustment = 1;
        }

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();
            Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.AddTemporaryMod(this.mod);
            Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.OnStatsChanged();
            yield return new WaitForSeconds(0.25f);
            yield return base.LearnAbility(0.25f);
            yield break;
        }

        private CardModificationInfo mod;
    }

}
