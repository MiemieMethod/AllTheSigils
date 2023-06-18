using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



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
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Regen_Full);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Regen_Full_a2);
            int powerlevel = 4;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_RegenFull.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_RegenFull), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_RegenFull.ability] = "ability_regen_full";
            }
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