using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddBoneShard()
        {
            // setup ability
            const string rulebookName = "Bone Shard";
            const string rulebookDescription = "[creature] will generate 1 bone when hit, if it lives through the attack.";
            const string LearnDialogue = "A splinter of bone.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 0);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_BoneShard_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_BoneShard");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_BoneShard), tex);

            // set ability to behaviour class
            void_BoneShard.ability = info.ability;


        }
    }

    public class void_BoneShard : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Health > 0;
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.1f);
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<ResourcesManager>.Instance.AddBones(1, base.Card.Slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}