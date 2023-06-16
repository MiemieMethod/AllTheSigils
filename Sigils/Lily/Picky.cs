// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddPicky()
        {
            AbilityInfo info = AbilityManager.New(
                          OldLilyPluginGuid,
                          "Picky",
                          "A Card bearing this sigil cannnot be summoned using any free cards as sacrifice.",
                          typeof(Picky),
                          GetTexture("picky")
                      );
            info.SetPixelAbilityIcon(GetTexture("picky", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Picky.ability = info.ability;
        }
    }

    public class Picky : AbilityBehaviour
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