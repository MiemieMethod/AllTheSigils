// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddLeft_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Left scratch",
                    "When a card bearing this sigil attacks it also attacks the space on the left of the attacked slot.",
                    typeof(Left_Slash),
                    GetTexture("left_scratch")
                );
            info.SetPixelAbilityIcon(GetTexture("left_scratch", true));
            info.powerLevel = 5;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Left_Slash.ability = info.ability;
        }
    }

    public class Left_Slash : AbilityBehaviour
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