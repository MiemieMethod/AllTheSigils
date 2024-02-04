using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.CardCosts;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AllTheSigils.Patches
{
    [HarmonyPatch]
    public class PlaceCard_patches
    {
        public static IEnumerator PatchedSelectSlotForCard(PlayableCard card)
        {
            Singleton<PlayerHand>.Instance.CardsInHand.ForEach(delegate (PlayableCard x)
            {
                x.SetEnabled(false);
            });
            yield return new WaitWhile(() => Singleton<PlayerHand>.Instance.ChoosingSlot);
            Singleton<PlayerHand>.Instance.OnSelectSlotStartedForCard(card);
            if (Singleton<RuleBookController>.Instance != null)
            {
                Singleton<RuleBookController>.Instance.SetShown(false, true);
            }
            Singleton<BoardManager>.Instance.CancelledSacrifice = false;
            Singleton<PlayerHand>.Instance.choosingSlotCard = card;
            if (card != null && card.Anim != null)
            {
                card.Anim.SetSelectedToPlay(true);
            }
            Singleton<BoardManager>.Instance.ShowCardNearBoard(card, true);
            if (Singleton<TurnManager>.Instance.SpecialSequencer != null)
            {
                yield return Singleton<TurnManager>.Instance.SpecialSequencer.CardSelectedFromHand(card);
            }
            bool cardWasPlayed = false;
            bool requiresSacrifices = card.Info.BloodCost > 0;
            if (requiresSacrifices)
            {
                List<CardSlot> validSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card != null);
                yield return Singleton<BoardManager>.Instance.ChooseSacrificesForCard(validSlots, card);
            }
            if (!Singleton<BoardManager>.Instance.CancelledSacrifice)
            {
                List<CardSlot> placeableSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => (x.Card == null || x.Card.HasAbility(Mount.ability) || card.HasAbility(Parasite.ability) || card.HasAbility(Hermit.ability)));
                if (card.HasAbility(Parasite.ability))
                {
                    placeableSlots.RemoveAll(x => x.Card == null);
                }
                if (card.HasAbility(Hermit.ability))
                {
                    placeableSlots.RemoveAll(x => !(x.Card?.HasTrait(Trait.Terrain) ?? true));
                }

                yield return Singleton<BoardManager>.Instance.ChooseSlot(placeableSlots, !requiresSacrifices);
                CardSlot lastSelectedSlot = Singleton<BoardManager>.Instance.LastSelectedSlot;
                if (lastSelectedSlot != null)
                {
                    cardWasPlayed = true;
                    card.Anim.SetSelectedToPlay(false);

                    if (lastSelectedSlot.Card == null)
                    {
                        yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(card, lastSelectedSlot);
                    }
                    else
                    {
                        if (Singleton<PlayerHand>.Instance.CardsInHand.Contains(card))
                        {
                            Singleton<PlayerHand>.Instance.RemoveCardFromHand(card);
                            if (card.TriggerHandler.RespondsToTrigger(Trigger.PlayFromHand, Array.Empty<object>()))
                            {
                                yield return card.TriggerHandler.OnTrigger(Trigger.PlayFromHand, Array.Empty<object>());
                            }

                            CardInfo cardInfo = lastSelectedSlot.Card.Info.Clone() as CardInfo;
                            CardModificationInfo mod = new CardModificationInfo();
                            if (lastSelectedSlot.Card.HasAbility(Mount.ability))
                            {
                                mod.negateAbilities.Add(Mount.ability);
                                mod.nameReplacement = $"{lastSelectedSlot.Card.Info.displayedName} mounted by {card.Info.displayedName}";
                            }
                            mod.abilities = card.Info.abilities;
                            if (card.HasAbility(Parasite.ability))
                            {
                                mod.abilities.Remove(Parasite.ability);
                                mod.nameReplacement = $"{lastSelectedSlot.Card.Info.displayedName} infested by {card.Info.displayedName}";
                            }
                            if (card.HasAbility(Hermit.ability))
                            {
                                mod.abilities.Remove(Hermit.ability);
                                mod.nameReplacement = $"{lastSelectedSlot.Card.Info.displayedName} inhabited by {card.Info.displayedName}";
                            }
                            mod.attackAdjustment = card.Attack;
                            mod.healthAdjustment = card.Health;
                            cardInfo.Mods.Add(mod);

                            lastSelectedSlot.Card.SetInfo(cardInfo);
                            lastSelectedSlot.Card.RenderCard();
                            card.ExitBoard(0f, new Vector3(0f, 0f, 0f));
                        }
                    }


                    //Inserting the API custom cost code here till we figure out a better solution
                    Dictionary<string, int> customCosts = new();
                    foreach (CustomCardCost cost1 in card.GetCustomCardCosts())
                        customCosts.Add(cost1.CostName, card.GetCustomCost(cost1.CostName));

                    yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(card, lastSelectedSlot);

                    if (card.Info.BonesCost > 0)
                    {
                        yield return Singleton<ResourcesManager>.Instance.SpendBones(card.Info.BonesCost);
                    }
                    if (card.EnergyCost > 0)
                    {
                        yield return Singleton<ResourcesManager>.Instance.SpendEnergy(card.EnergyCost);
                    }

                    //Inserting the API custom cost code here till we figure out a better solution
                    foreach (var cost2 in card.GetCustomCardCosts())
                    {
                        if (customCosts.ContainsKey(cost2.CostName))
                            yield return cost2.OnPlayed(customCosts[cost2.CostName], card);
                    }
                }
            }
            if (!cardWasPlayed)
            {
                Singleton<BoardManager>.Instance.ShowCardNearBoard(card, false);
            }
            Singleton<PlayerHand>.Instance.choosingSlotCard = null;
            if (card != null && card.Anim != null)
            {
                card.Anim.SetSelectedToPlay(false);
            }
            Singleton<PlayerHand>.Instance.CardsInHand.ForEach(delegate (PlayableCard x)
            {
                x.SetEnabled(true);
            });
            yield break;
        }

        [HarmonyPatch(typeof(PlayerHand), "SelectSlotForCard")]
        [HarmonyPostfix]
        public static bool Prefix(PlayableCard card, out IEnumerator __result)
        {
            __result = PatchedSelectSlotForCard(card);
            return false;
        }

        [HarmonyPatch(typeof(BoardManager), "SacrificesCreateRoomForCard")]
        [HarmonyPostfix]
        public static void SacrificesCreateRoomForCardPostFix(PlayableCard card, ref List<CardSlot> sacrifices, ref bool __result, ref BoardManager __instance)
        {
            //god it was complicated figuring this all out
            if (__instance.PlayerSlotsCopy.Any(x => x.Card?.HasAbility(Mount.ability) ?? false))
            {
                __result = true;
                return;
            }

            if (card.HasAbility(Parasite.ability))
            {
                int amountOfCardsThatCanBeRemovedBySacrifice = __instance.PlayerSlotsCopy.Where(x => SigilEffectUtils.cardWillBeRemovedIfSacrificed(x.Card)).ToList().Count;
                if (card.Info.BloodCost > 0)
                {
                    if (amountOfCardsThatCanBeRemovedBySacrifice > card.Info.BloodCost)
                    {
                        __result = true;
                        return;
                    }
                }
                else
                {
                    if (__instance.PlayerSlotsCopy.Any(x => x.Card != null))
                    {
                        __result = true;
                        return;
                    }
                }
                __result = false;
                return;
            }

            if (card.HasAbility(Hermit.ability))
            {
                if (__instance.PlayerSlotsCopy.Any(x => x.Card?.HasTrait(Trait.Terrain) ?? false))
                {
                    __result = true;
                    return;
                }
            }

            if (sacrifices.Any(x => x.Card?.HasAbility(Resourceful.ability) ?? false) && __result == true)
            {
                foreach (CardSlot cardSlot in __instance.PlayerSlotsCopy)
                {
                    if (cardSlot.Card == null)
                    {
                        return;
                    }
                    if (card.Info.BloodCost >= 1 && sacrifices.Contains(cardSlot) && SigilEffectUtils.cardWillBeRemovedIfSacrificed(cardSlot.Card))
                    {
                        return;
                    }
                }
                __result = false;
                return;
            }
        }
    }
}
