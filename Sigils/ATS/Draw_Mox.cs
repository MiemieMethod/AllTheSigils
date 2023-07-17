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
        public void AddDraw_Mox()
        {
            AbilityInfo info = AbilityManager.New(
                    PluginGuid,
                    "Draw Mox",
                    "When [creature] is played, a random card costing mox is created in your hand.",
                    typeof(Draw_Mox),
                    GetTexture("draw_mox")
                );
            info.SetPixelAbilityIcon(GetTexture("draw_mox", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Draw_Mox.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("draw_mox", "");
            }
        }
    }

    public class Draw_Mox : DrawCreatedCard
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
                CardInfo info = GetRandomChoosableCardWithMox(base.GetRandomSeed());
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

        public static CardInfo GetRandomChoosableCardWithMox(int randomSeed)
        {
            CardMetaCategory metaCategory = SaveManager.SaveFile.IsPart2 ? CardMetaCategory.GBCPack : CardMetaCategory.ChoiceNode;
            List<CardInfo> list = CardLoader.GetUnlockedCards(metaCategory, CardTemple.Nature).FindAll((CardInfo x) => x.gemsCost.Count > 0);
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