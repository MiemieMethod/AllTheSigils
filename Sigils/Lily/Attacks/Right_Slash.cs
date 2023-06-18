// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddRight_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Right scratch",
                    "When [creature] attacks it also attacks the space on the right of the attacked slot.",
                    typeof(Right_Slash),
                    GetTexture("right_scratch")
                );
            info.SetPixelAbilityIcon(GetTexture("right_scratch", true));
            info.powerLevel = 5;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Right_Slash.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "right_scratch";
            }
        }
    }

    public class Right_Slash : AbilityBehaviour
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