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
        public void AddMount()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Mount",
                    "Cards may be played on top of [creature]. If they are their sigils and stats will be combined, and a name combination will be created.",
                    typeof(Mount),
                    GetTexture("mount")
                );
            info.SetPixelAbilityIcon(GetTexture("mount", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Mount.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("mount", "When a creature is placed on top of a card bearing this sigil, the mount sigil will be removed from it.");
            }
        }
    }

    public class Mount : AbilityBehaviour
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