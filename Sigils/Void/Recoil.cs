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
        private void AddRecoil()
        {
            // setup ability
            const string rulebookName = "Recoil";
            const string rulebookDescription = "[creature] will take 1 damage each time they attack.";
            const string LearnDialogue = "The strength causes the creature pain.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1, Plugin.configDying.Value);
            info.canStack = false;
            info.flipYIfOpponent = true;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_recoil_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Recoil");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Recoil), tex);

            // set ability to behaviour class
            void_Recoil.ability = info.ability;


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