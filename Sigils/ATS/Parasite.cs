using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddParasite()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Parasite",
                    "[creature] may be placed on top of any card. If it is their sigils and stats will be combined, and a name combination will be created.",
                    typeof(Parasite),
                    GetTexture("parasite")
                );
            info.SetPixelAbilityIcon(GetTexture("parasite", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Parasite.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("parasite", "");
            }
        }
    }

    public class Parasite : AbilityBehaviour
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