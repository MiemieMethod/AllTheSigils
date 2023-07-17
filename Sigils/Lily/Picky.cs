// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
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
                          GetTextureLily("picky")
                      );
            info.SetPixelAbilityIcon(GetTextureLily("picky", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Picky.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("picky", "");
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