// Using Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
// Modding Inscryption
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddFiveDeathBones()
        {
            AbilityInfo info = AbilityManager.New(
                          OldLilyPluginGuid,
                          "Bone lord 5",
                          "When a card bearing this sigil dies, 5 bones are rewarded instead of 1.",
                          typeof(FiveDeathBones),
                          GetTexture("fivedeathbones")
                      );
            info.SetPixelAbilityIcon(GetTexture("fivedeathbones", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            FiveDeathBones.ability = info.ability;
        }
    }

    public class FiveDeathBones : AbilityBehaviour
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
            yield return Singleton<ResourcesManager>.Instance.AddBones(4, base.Card.Slot);
            yield return base.LearnAbility(0f);
            yield return new WaitForSeconds(0.25f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}