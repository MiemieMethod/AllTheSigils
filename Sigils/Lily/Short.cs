// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddShort()
        {
            AbilityInfo info = AbilityManager.New(
                       OldLilyPluginGuid,
                       "Short",
                       "[creature] will not be blocked by an opposing creature bearing the airborn sigil.",
                       typeof(Short),
                       GetTextureLily("short")
                   );
            info.SetPixelAbilityIcon(GetTextureLily("short", true));
            info.powerLevel = 0;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Short.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("short", "");
            }
        }
    }

    public class Short : AbilityBehaviour
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