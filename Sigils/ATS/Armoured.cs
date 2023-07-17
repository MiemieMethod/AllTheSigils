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
        public void AddArmoured()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Shielded",
                    "When [creature] is dealt damage, the damage dealt is halved, rounded down.",
                    typeof(Armoured),
                    GetTexture("shielded")
                );
            info.SetPixelAbilityIcon(GetTexture("shielded", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Armoured.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("shielded", "");
            }
        }
    }

    public class Armoured : AbilityBehaviour
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