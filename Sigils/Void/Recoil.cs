using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddRecoil()
        {
            // setup ability
            const string rulebookName = "Recoil";
            const string rulebookDescription = "[creature] will take 1 damage each time they attack.";
            const string LearnDialogue = "The strength causes the creature pain.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Recoil);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Recoil_a2);
            int powerlevel = -1;
            bool LeshyUsable = Plugin.configRecoil.Value;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Recoil.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Recoil), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Recoil.ability] = "void_Recoil";
            }
        }
    }

    public class void_Recoil : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToAttackEnded()
        {
            return base.Card.HasAbility(void_Recoil.ability);
        }

        public override IEnumerator OnAttackEnded()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.55f);
            base.Card.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.35f);
            yield return base.Card.TakeDamage(1, null);
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}