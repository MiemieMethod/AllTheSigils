// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddLinguist()
        {
            AbilityInfo info = AbilityManager.New(
                       OldLilyPluginGuid,
                       "Linguist",
                       "While [creature] is on the board, all talking cards on your side of the board get +1 attack.",
                       typeof(Linguist),
                       GetTextureLily("linguist")
                   );
            info.SetPixelAbilityIcon(GetTextureLily("placeholder", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Linguist.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("linguist", "");
            }
        }
    }

    public class Linguist : AbilityBehaviour
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