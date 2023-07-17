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
        public void AddSympathetic()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Sympathetic",
                    "[creature] will only attack if the opposing creature has more or equal health to it.",
                    typeof(Sympathetic),
                    GetTexture("sympathetic")
                );
            info.SetPixelAbilityIcon(GetTexture("sympathetic", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Sympathetic.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("sympathetic", "");
            }
        }
    }

    public class Sympathetic : AbilityBehaviour
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