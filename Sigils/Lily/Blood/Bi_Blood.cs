// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddBi_Blood()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Noble Sacrifice",
                    "[creature] is counted as 2 blood rather than 1 blood when sacrificed.",
                    typeof(Bi_Blood),
                    GetTexture("bi_blood")
                );
            info.SetPixelAbilityIcon(GetTexture("bi_blood", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Bi_Blood.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "bi_blood";
            }
        }
    }

    public class Bi_Blood : AbilityBehaviour
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