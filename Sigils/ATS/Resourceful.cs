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
        public void AddResourceful()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Resourceful",
                    "When [creature] is sacrificed, instead of dying, it loses a random sigil. Resourceful will only remove itself when there are no other sigils on the card.",
                    typeof(Resourceful),
                    GetTexture("resourceful")
                );
            info.SetPixelAbilityIcon(GetTexture("resourceful", true));
            info.powerLevel = 3; //not final
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Resourceful.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("resourceful", "");
            }
        }
    }

    public class Resourceful : AbilityBehaviour
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