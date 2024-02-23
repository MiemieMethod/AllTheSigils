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
using System.Text;
using UnityEngine;
using static InscryptionAPI.Card.SpecialTriggeredAbilityManager;

namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddChangePortraitWhenMerged()
        {
            FullSpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(
                Plugin.PluginGuid,
                "ChangePortraitWhenMerged",
                typeof(ChangePortraitWhenMerged)
                );

            ChangePortraitWhenMerged.ability = specialAbility.Id;
        }

        public class ChangePortraitWhenMerged : SpecialCardBehaviour
        {
            public static SpecialTriggeredAbility ability;

            public void Update()
            {
                if (base.Card.Info.HasModFromCardMerge())
                {
                    if (base.Card.RenderInfo.portraitOverride != base.Card.Info.alternatePortrait)
                    {
                        base.Card.RenderInfo.portraitOverride = base.Card.Info.alternatePortrait;
                        base.Card.RenderCard();
                    }
                }
            }
        }
    }
}
