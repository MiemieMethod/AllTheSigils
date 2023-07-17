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
        public void AddNurse()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Nurse",
                    "At the end of the opponent's turn, [creature] will heal any adjacent friendly cards.",
                    typeof(Nurse),
                    GetTexture("nurse")
                );
            info.SetPixelAbilityIcon(GetTexture("nurse", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Nurse.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("nurse", "");
            }
        }
    }

    public class Nurse : AbilityBehaviour
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
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
            if (toLeftSlot?.Card != null || toRightSlot?.Card != null)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board);
                yield return new WaitForSeconds(0.2f);
            }

            if (toLeftSlot?.Card != null)
            {
                toLeftSlot.Card.HealDamage(1);
            }
            if (toRightSlot?.Card != null)
            {
                toRightSlot.Card.HealDamage(1);
            }

            if (toLeftSlot?.Card != null || toRightSlot?.Card != null)
            {
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
}