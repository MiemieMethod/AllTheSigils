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
        public void AddHermit()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Hermit",
                    "[creature] may be placed on any terrain card. If it is their sigils and stats will be combined, and a name combination will be created.",
                    typeof(Hermit),
                    GetTexture("hermit")
                );
            info.SetPixelAbilityIcon(GetTexture("hermit", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Hermit.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("hermit", "");
            }
        }
    }

    public class Hermit : AbilityBehaviour
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