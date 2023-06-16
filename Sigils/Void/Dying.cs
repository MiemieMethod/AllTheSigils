using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddDying()
        {
            // setup ability
            const string rulebookName = "Dying";
            const string rulebookDescription = "[creature] will lose 1 health each time it declares an attack.";
            const string LearnDialogue = "Tik Toc";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1, Plugin.configDying.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_dying"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_dying");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_dying), tex);

            // set ability to behaviour class
            void_dying.ability = info.ability;


        }
    }

    public class void_dying : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return attacker.HasAbility(void_dying.ability);
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.55f);
            attacker.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.35f);
            yield return attacker.TakeDamage(1, null);
            Plugin.Log.LogWarning("Dying debug " + attacker + " has taken damage");
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}