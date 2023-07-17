// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddPuppets_Gift()
        {
            AbilityInfo info = AbilityManager.New(
                          OldLilyPluginGuid,
                          "Puppets gift",
                          "As long as [creature] is on the board any cards with brittle won't die because of brittle.",
                          typeof(Puppets_Gift),
                          GetTextureLily("puppets_gift")
                      );
            info.SetPixelAbilityIcon(GetTextureLily("puppets_gift", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Puppets_Gift.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("puppets_gift", "");
            }
        }
    }

    public class Puppets_Gift : AbilityBehaviour
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