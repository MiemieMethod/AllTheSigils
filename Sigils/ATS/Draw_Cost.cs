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
        public void AddDraw_Cost()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Draw Cost",
                    "When [creature] is played, a random card that has the same cost type as [creature] is created in your hand.",
                    typeof(Draw_Cost),
                    GetTexture("draw_cost")
                );
            info.SetPixelAbilityIcon(GetTexture("draw_cost", true));
            info.powerLevel = 4;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Draw_Cost.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("draw_cost", "");
            }
        }
    }

    public class Draw_Cost : DrawCreatedCard
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo info = GetRandomChoosableCardWithCost(base.GetRandomSeed());
                if (info != null)
                {
                    return CardLoader.GetCardByName(info.name);
                }
                return null;
            }
        }

        public override bool RespondsToResolveOnBoard()
        {
            return CardToDraw != null;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (Singleton<ViewManager>.Instance.CurrentView != this.DrawCardView)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(this.DrawCardView, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(this.CardToDraw, base.Card.TemporaryMods, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }

        public CardInfo GetRandomChoosableCardWithCost(int randomSeed)
        {
            List<CardInfo> list = new List<CardInfo>();
            CardMetaCategory metaCategory = SaveManager.SaveFile.IsPart2 ? CardMetaCategory.GBCPack : CardMetaCategory.ChoiceNode;

            List<CardInfo> cardList = CardLoader.GetUnlockedCards(metaCategory, CardTemple.Nature);
            if (base.Card.Info.BloodCost > 0)
            {
                list.AddRange(cardList.FindAll((CardInfo x) => x.BloodCost > 0));
            }
            if (base.Card.Info.BonesCost > 0)
            {
                list.AddRange(cardList.FindAll((CardInfo x) => x.BonesCost > 0));
            }
            if (base.Card.EnergyCost > 0)
            {
                list.AddRange(cardList.FindAll((CardInfo x) => x.EnergyCost > 0));
            }
            if (base.Card.Info.GemsCost != null || (base.Card.Info.GemsCost.Count > 0))
            {
                list.AddRange(cardList.FindAll((CardInfo x) => x.gemsCost != null || (x.gemsCost.Count > 0)));
            }
            if (list.Count == 0)
            {
                list.AddRange(cardList.FindAll((CardInfo x) => x.BloodCost == 0 && x.BonesCost == 0 && x.EnergyCost == 0 && (x.gemsCost == null || x.gemsCost.Count == 0)));
            }
            list = list.Distinct().ToList();

            if (list.Count > 0)
            {
                return CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
            }
            else
            {
                return null;
            }
        }
    }
}