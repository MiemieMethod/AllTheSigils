// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddFearful()
        {
            AbilityInfo info = AbilityManager.New(
                OldLilyPluginGuid,
                "Fearful",
                "When [creature] is struck without it resulting in death, it will be returned to its owner's hand.",
                typeof(Fearful),
                GetTextureLily("fearful")
            );
            info.SetPixelAbilityIcon(GetTextureLily("fearful", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Fearful.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("fearful", "");
            }
        }
    }

    public class Fearful : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            if (target == base.Card)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            if (amount < target.Info.Health)
            {
                CardModificationInfo mod = new CardModificationInfo() { healthAdjustment = (target.Info.baseHealth - target.Info.Health) - amount };
                List<CardModificationInfo> modlist = new List<CardModificationInfo>() { mod };
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(base.Card.Info, modlist, 0.25f, null);
                target.ExitBoard(0, new Vector3(0, 0, 0));
                yield return new WaitForSeconds(0.45f);
                yield break;
            }
        }
    }
}