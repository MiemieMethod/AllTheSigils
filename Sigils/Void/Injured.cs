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
        private void AddInjured()
        {
            // setup ability
            const string rulebookName = "Injured";
            const string rulebookDescription = "[creature] is hurt and will lose 1 health each time it declares an attack due to the strain of the injuries.";
            const string LearnDialogue = "Tik Toc";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Injured);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Injured_a2);
            int powerlevel = -1;
            bool LeshyUsable = Plugin.configDying.Value;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Injured.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Injured), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Injured.ability] = new Tuple<string, string>("void_Injured", "");
            }
        }
    }

    public class void_Injured : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return attacker.HasAbility(void_Injured.ability);
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.55f);
            attacker.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.35f);
            yield return attacker.TakeDamage(1, null);
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}