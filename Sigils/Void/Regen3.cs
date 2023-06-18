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
        private void AddRegen3()
        {
            // setup ability
            const string rulebookName = "Regen 3";
            const string rulebookDescription = "At the end of the owner's turn, [creature] will regen 3 health.";
            const string LearnDialogue = "This creature will heal 3 Health at the end of it's owner's turn.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Regen_3);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Regen_3_a2);
            int powerlevel = 3;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Regen3.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Regen3), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Regen3.ability] = "ability_regen_3";
            }
        }
    }

    public class void_Regen3 : AbilityBehaviour
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
            if (base.Card.Status.damageTaken > 0)
            {
                base.Card.HealDamage(3);
            }
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}