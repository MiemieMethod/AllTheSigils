// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using InscryptionAPI.Card;

using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddTwoSummonBones()
        {
            AbilityInfo info = AbilityManager.New(
                                OldLilyPluginGuid,
                                "Bone hoarder 2",
                                "When [creature] is played, 2 bones are rewarded.",
                                typeof(TwoSummonBones),
                                GetTexture("twosummonbones")
                            );
            info.SetPixelAbilityIcon(GetTexture("twosummonbones", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            TwoSummonBones.ability = info.ability;
        }
    }

    public class TwoSummonBones : AbilityBehaviour
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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        // Token: 0x060013EE RID: 5102 RVA: 0x000441EB File Offset: 0x000423EB
        public override IEnumerator OnResolveOnBoard()
        {
            yield return Singleton<ResourcesManager>.Instance.AddBones(1, base.Card.Slot);
            yield break;
        }
    }
}