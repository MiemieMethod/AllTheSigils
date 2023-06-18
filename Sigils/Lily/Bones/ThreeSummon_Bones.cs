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
        public void AddThreeSummonBones()
        {
            AbilityInfo info = AbilityManager.New(
                                OldLilyPluginGuid,
                                "Bone hoarder 3",
                                "When [creature] is played, 3 bones are rewarded.",
                                typeof(ThreeSummonBones),
                                GetTexture("threesummonbones")
                            );
            info.SetPixelAbilityIcon(GetTexture("threesummonbones", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            ThreeSummonBones.ability = info.ability;
        }
    }

    public class ThreeSummonBones : AbilityBehaviour
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
            yield return Singleton<ResourcesManager>.Instance.AddBones(3, base.Card.Slot);
            yield break;
        }
    }
}