// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddQuadra_Blood()
        {
            AbilityInfo info = AbilityManager.New(
                             OldLilyPluginGuid,
                             "Superior Sacrifice",
                             "[creature] is counted as 4 blood rather than 1 blood when sacrificed.",
                             typeof(Quadra_Blood),
                             GetTexture("quadra_blood")
                         );
            info.SetPixelAbilityIcon(GetTexture("quadra_blood", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Quadra_Blood.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "quadra_blood";
            }
        }
    }

    public class Quadra_Blood : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}