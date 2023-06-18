using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



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
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_BoneShard);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_BoneShard_a2);
            int powerlevel = 0;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = true;

            // set ability to behaviour class
            void_BoneShard.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_BoneShard), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_BoneShard.ability] = "void_BoneShard";
            }
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