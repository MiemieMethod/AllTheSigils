// Using Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
// Modding Inscryption
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddBond()
        {
            AbilityInfo info = AbilityManager.New(
                       OldLilyPluginGuid,
                       "Bond",
                       "When a creature bearing this sigil has a adjacent creature it will gain +1 attack/health dependent on which side the adjacent creature is.",
                       typeof(Bond),
                       GetTexture("Bond")
                   );
            info.SetPixelAbilityIcon(GetTexture("bond", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Bond.ability = info.ability;
        }
    }

    public class Bond : AbilityBehaviour
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