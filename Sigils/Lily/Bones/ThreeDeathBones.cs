// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddThreeDeathBones()
        {
            AbilityInfo info = AbilityManager.New(
                                   OldLilyPluginGuid,
                                   "Bone prince 3",
                                   "When [creature] dies, 3 bones are rewarded instead of 1.",
                                   typeof(ThreeDeathBones),
                                   GetTextureLily("threedeathbones")
                               );
            info.SetPixelAbilityIcon(GetTextureLily("threedeathbones", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            ThreeDeathBones.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("threedeathbones", "");
            }
        }
    }

    public class ThreeDeathBones : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        // Token: 0x060013ED RID: 5101 RVA: 0x0000F57E File Offset: 0x0000D77E
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }

        // Token: 0x060013EE RID: 5102 RVA: 0x000441EB File Offset: 0x000423EB
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<ResourcesManager>.Instance.AddBones(2, base.Card.Slot);
            yield return base.LearnAbility(0f);
            yield return new WaitForSeconds(0.25f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}