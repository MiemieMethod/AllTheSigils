// Using Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
// Modding Inscryption
using System;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddAll_Seeing()
        {
            AbilityInfo info = AbilityManager.New(
                OldLilyPluginGuid,
                "All seeing",
                "While [creature] is on the board, all talking cards on your side of the board get +2 health.",
                typeof(All_Seeing),
                GetTextureLily("all_seeing")
            );
            info.SetPixelAbilityIcon(GetTextureLily("placeholder", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            All_Seeing.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("all_seeing", "");
            }
        }
    }

    public class All_Seeing : AbilityBehaviour
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