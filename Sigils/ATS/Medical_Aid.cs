using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using Pixelplacement;
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
        public void AddMedical_Aid()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Medical Aid",
                    "At the end of the opponent's turn, choose a hurt creature on the owner's side to heal for 1.",
                    typeof(Medical_Aid),
                    GetTexture("medical_aid")
                );
            info.SetPixelAbilityIcon(GetTexture("medical_aid", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Medical_Aid.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("medical_aid", "");
            }
        }
    }

    public class Medical_Aid : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            List<CardSlot> allTargets = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
            List<CardSlot> validTargets = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card != null && !x.Card.Dead && x.Card.Status.damageTaken > 0);

            yield return SigilEffectUtils.chooseSlot(allTargets, validTargets, targetChosen);
            yield break;
        }

        public void targetChosen(CardSlot slot)
        {
            slot.Card.HealDamage(1);
        }
    }
}