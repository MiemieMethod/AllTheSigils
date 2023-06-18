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
                          "[creature] cannnot be summoned by using any free cards as sacrifice.",
                          typeof(Picky),
                          GetTexture("picky")
                      );
            info.SetPixelAbilityIcon(GetTexture("picky", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Picky.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "picky";
            }
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