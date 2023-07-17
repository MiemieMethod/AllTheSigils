using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddBlight()
        {
            // setup ability
            const string rulebookName = "Blight";
            const string rulebookDescription = "When [creature] is sacrificed, it subtracts its stat values to the card it was sacrificed for.";
            const string LearnDialogue = "A disease shouldnt spread.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Blight);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Blight_a2);
            int powerlevel = -5;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Blight.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Blight), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Blight.ability] = new Tuple<string, string>("void_blight", "");
            }
        }
    }

    public class void_Blight : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        private void Start()
        {
            this.mod = new CardModificationInfo();
        }

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();

            if (Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.MaxHealth > base.Card.MaxHealth)
            {
                this.mod.healthAdjustment = base.Card.MaxHealth * -1;
                this.mod.attackAdjustment = base.Card.Attack * -1;
                Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.AddTemporaryMod(this.mod);
                Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.OnStatsChanged();
            }
            else
            {
                this.mod.healthAdjustment = (Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.MaxHealth - 1) * -1;
                this.mod.attackAdjustment = base.Card.Attack * -1;
                Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.AddTemporaryMod(this.mod);
                Singleton<BoardManager>.Instance.currentSacrificeDemandingCard.OnStatsChanged();
            }

            yield return new WaitForSeconds(0.25f);
            yield return base.LearnAbility(0.25f);
            yield break;
        }

        private CardModificationInfo mod;

    }
}