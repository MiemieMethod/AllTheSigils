// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddDouble_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                             OldLilyPluginGuid,
                             "Double scratch",
                             "When [creature] attacks it attacks twice and the space right and left of the attacked slot.",
                             typeof(Double_Slash),
                             GetTexture("double_scratch")
                         );
            info.SetPixelAbilityIcon(GetTexture("double_scratch", true));
            info.powerLevel = 7;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Double_Slash.ability = info.ability;
        }
    }

    public class Double_Slash : AbilityBehaviour
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