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
        public void AddUnderdog()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Underdog",
                    "[creature] can only be played when you are losing.",
                    typeof(Underdog),
                    GetTexture("underdog")
                );
            info.SetPixelAbilityIcon(GetTexture("underdog", true));
            info.powerLevel = -1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Underdog.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("underdog", "");
            }
        }
    }

    public class Underdog : AbilityBehaviour
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