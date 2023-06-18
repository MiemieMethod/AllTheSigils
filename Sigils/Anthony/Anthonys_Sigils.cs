using APIPlugin;
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
using Random = UnityEngine.Random;

namespace AllTheSigils
{
    public partial class Plugin
    {
        public static void AimWeaponAnim(GameObject TweenObj, Vector3 target)
        {
            Tween.LookAt(TweenObj.transform, target, Vector3.up, 0.075f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
        }
        private void AddActivactedNanoShield()
        {
            AbilityInfo info = AbilityManager.New(
                    OldAnthonyPluginGuid,
                    "Activated Latch Nano Shield",
                    "When activated for a cost of 1 energy / 2 bones will allow the owner to give a creature Nano Shield.",
                    typeof(Bi_Blood),
                    GetTextureAnthony("activatedlatch-deathshield")
                );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.activated = true;
            info.opponentUsable = false;

            ActivatedLatchNanoShield.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "activatedlatch-deathshield";
            }
        }
        private void AddActivactedBrittle()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Activated Latch Brittle",
                       "When activated for a cost of 1 energy will allow the owner to give a creature Brittle.",
                       typeof(ActivatedLatchBrittle),
                       GetTextureAnthony("activatedlatch-brittle")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.activated = true;
            info.opponentUsable = false;

            ActivatedLatchBrittle.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "activatedlatch-brittle";
            }
        }
        private void AddActivactedExplodeOnDeath()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Activated Latch Explode On Death",
                       "When activated for a cost of 1 energy will allow the owner to give a creature Explode On Death.",
                       typeof(ActivatedLatchExplodeOnDeath),
                       GetTextureAnthony("activatedlatch-explodeondeath")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.activated = true;
            info.opponentUsable = false;

            ActivatedLatchExplodeOnDeath.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "activatedlatch-explodeondeath";
            }
        }
        private void AddActivactedReach()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Activated Latch Reach",
                       "When activated for a cost of 2 energy will allow the owner to give a creature Reach.",
                       typeof(ActivatedLatchReach),
                       GetTextureAnthony("acticatedlatch-Reach")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.activated = true;
            info.opponentUsable = false;

            ActivatedLatchReach.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "acticatedlatch-Reach";
            }
        }
        private void AddIncreasePowerDecreaseHealth()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Old Timer",
                       "At the Start of its owner's turn [creature] will gain +1 attack and will take 1 damage.",
                       typeof(IncreasePowerDecreaseHealth),
                       GetTextureAnthony("old-timer")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            IncreasePowerDecreaseHealth.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "old-timer";
            }
        }
        private void AddDecreasePowerIncreaseHealth()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Docile",
                       "At the Start of its owner's turn [creature] will lose -1 attack and will gain +1 health.",
                       typeof(DecreasePowerIncreaseHealth),
                       GetTextureAnthony("docile")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            DecreasePowerIncreaseHealth.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "docile";
            }
        }
        private void AddEatChicken()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "To The Slaughter",
                       "At the end of each turn [creature] will eat a random Chicken gaining the same amount of attack as the eaten chicken's health, and gaining the same amount of health as the eaten chicken's attack.",
                       typeof(EatChickens),
                       GetTextureAnthony("eatchicken")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            EatChickens.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "eatchicken";
            }
        }
        private void AddChickenCard()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            List<CardAppearanceBehaviour.Appearance> list = new List<CardAppearanceBehaviour.Appearance>();
            list.Add(CardAppearanceBehaviour.Appearance.GoldEmission);
            List<Ability> abilities = new List<Ability>
            {
                JustChicken.ability
            };
            CardInfo cardInfo = ScriptableObject.CreateInstance<CardInfo>();
            cardInfo.name = "Chicken";
            cardInfo.displayedName = "Chicken";
            cardInfo.baseAttack = 1;
            cardInfo.baseHealth = 1;
            cardInfo.cost = 0;
            cardInfo.SetPortrait(GetTextureAnthony("YisusChickenPortrait"));
            cardInfo.metaCategories = metaCategories;
            cardInfo.appearanceBehaviour = list;
            cardInfo.cardComplexity = CardComplexity.Simple;
            cardInfo.abilities = abilities;
            cardInfo.temple = CardTemple.Nature;
            CardManager.Add("AP", cardInfo);
        }
        private void AddChicken()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Chicken",
                       "[creature] counts as a Chicken.",
                       typeof(JustChicken),
                       GetTextureAnthony("chicken")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            JustChicken.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "chicken";
            }
        }
        private void AddTransformChickenOpp()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Transform Chicken (Opposing)",
                       "When a creature is played opposing [creature] it will turn into a Chicken. A Chicken is defined as 1 Attack, 1 Health.",
                       typeof(TurnIntoChickenOpp),
                       GetTextureAnthony("TransformChick_Sigil")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            TurnIntoChickenOpp.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "TransformChick_Sigil";
            }
        }
        private void AddTransformChickenLooseCannon()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Transform Chicken (Loose Cannon)",
                       "[creature] will transform a random creature on the board into a Chicken. A Chicken is defined as 1 Attack, 1 Health.",
                       typeof(TurnIntoChickenLooseCannon),
                       GetTextureAnthony("TransformChick_Sigil")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            TurnIntoChickenLooseCannon.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "TransformChick_Sigil";
            }
        }
        private void AddTransformChickenEnemyOnly()
        {
            AbilityInfo info = AbilityManager.New(
                       OldAnthonyPluginGuid,
                       "Transform Chicken (Enemy Only)",
                       "[creature] will transform a random creature on the board on the opponent's side into a Chicken. A Chicken is defined as 1 Attack, 1 Health.",
                       typeof(TurnIntoChickenEnemyOnly),
                       GetTextureAnthony("TransformChick_Sigil")
                   );
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook };
            info.canStack = false;
            info.opponentUsable = true;

            TurnIntoChickenEnemyOnly.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "TransformChick_Sigil";
            }
        }
        public class TurnIntoChickenOpp : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return this.Ability;
                }
            }
            public override int Priority
            {
                get
                {
                    return -1;
                }
            }
            public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
            {
                return this.RespondsToTrigger(otherCard);
            }
            public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
            {
                yield return this.FireAtOpposingSlot(otherCard);
                yield break;
            }
            public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
            {
                return this.RespondsToTrigger(otherCard);
            }
            public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
            {
                yield return this.FireAtOpposingSlot(otherCard);
                yield break;
            }
            private bool RespondsToTrigger(PlayableCard otherCard)
            {
                return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
            }
            private IEnumerator FireAtOpposingSlot(PlayableCard otherCard)
            {
                if (otherCard != this.lastShotCard || Singleton<TurnManager>.Instance.TurnNumber != this.lastShotTurn)
                {
                    this.lastShotCard = otherCard;
                    this.lastShotTurn = Singleton<TurnManager>.Instance.TurnNumber;
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return new WaitForSeconds(0.25f);
                    if (otherCard != null && !otherCard.Dead)
                    {
                        CardInfo cardByName = CardLoader.GetCardByName("AP_Chicken");
                        yield return otherCard.TransformIntoCard(cardByName, null);
                    }
                    yield return base.LearnAbility(0.5f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                yield break;
            }
            public static Ability ability;
            private int lastShotTurn = -1;
            private PlayableCard lastShotCard;
        }
        public class TurnIntoChickenLooseCannon : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.TurnIntoChickenLooseCannon.ability;
                }
            }
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.HasAbility(Plugin.TurnIntoChickenLooseCannon.ability);
            }
            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                PlayableCard card = base.Card;
                List<CardSlot> allSlots = Singleton<BoardManager>.Instance.AllSlots;
                List<PlayableCard> list = new List<PlayableCard>();
                int num;
                for (int i = 0; i < allSlots.Count; i = num + 1)
                {
                    if (allSlots[i].Card != null)
                    {
                        list.Add(allSlots[i].Card);
                    }
                    num = i;
                }
                PlayableCard target = list[Random.Range(0, list.Count)];
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                CardInfo cardByName = CardLoader.GetCardByName("AP_Chicken");
                yield return target.TransformIntoCard(cardByName, null);
                yield return base.LearnAbility(0.25f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break;
            }
            public static Ability ability;
        }
        public class TurnIntoChickenEnemyOnly : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.TurnIntoChickenEnemyOnly.ability;
                }
            }
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.HasAbility(Plugin.TurnIntoChickenEnemyOnly.ability);
            }
            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                PlayableCard card = base.Card;
                List<CardSlot> opponentSlots = Singleton<BoardManager>.Instance.opponentSlots;
                List<PlayableCard> list = new List<PlayableCard>();
                int num;
                for (int i = 0; i < opponentSlots.Count; i = num + 1)
                {
                    if (opponentSlots[i].Card != null)
                    {
                        list.Add(opponentSlots[i].Card);
                    }
                    num = i;
                }
                PlayableCard target = list[Random.Range(0, list.Count)];
                if (target != null)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    card.Anim.LightNegationEffect();
                    yield return base.PreSuccessfulTriggerSequence();
                    CardInfo cardByName = CardLoader.GetCardByName("AP_Chicken");
                    yield return target.TransformIntoCard(cardByName, null);
                    yield return base.LearnAbility(0.25f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                yield break;
            }
            public static Ability ability;
        }
        public class EatChickens : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.EatChickens.ability;
                }
            }
            private void Start()
            {
                this.mod = new CardModificationInfo();
                this.mod.nonCopyable = true;
                this.mod.singletonId = "increaseATKincreaseHealth";
                this.mod.attackAdjustment = 0;
                this.mod.healthAdjustment = 0;
                base.Card.AddTemporaryMod(this.mod);
            }
            public override bool RespondsToTurnEnd(bool playerTurnEnd)
            {
                return base.Card.HasAbility(Plugin.EatChickens.ability);
            }
            public override IEnumerator OnTurnEnd(bool playerTurnEnd)
            {
                PlayableCard card = base.Card;
                List<CardSlot> allSlots = Singleton<BoardManager>.Instance.AllSlots;
                List<PlayableCard> list = new List<PlayableCard>();
                int num;
                for (int i = 0; i < allSlots.Count; i = num + 1)
                {
                    if (allSlots[i].Card != null && allSlots[i].Card.HasAbility(Plugin.JustChicken.ability))
                    {
                        list.Add(allSlots[i].Card);
                    }
                    num = i;
                }
                PlayableCard target = list[Random.Range(0, list.Count)];
                if (target != null)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    card.Anim.LightNegationEffect();
                    yield return base.PreSuccessfulTriggerSequence();
                    this.mod.attackAdjustment += target.Info.Health;
                    this.mod.healthAdjustment += target.Info.Attack;
                    base.Card.OnStatsChanged();
                    target.TakeDamage(1024, card);
                    target.Die(false, card, false);
                    yield return base.LearnAbility(0.25f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                yield break;
            }
            public static Ability ability;
            private CardModificationInfo mod;
        }
        public class JustChicken : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.JustChicken.ability;
                }
            }
            public static Ability ability;
        }
        public abstract class ActivatedLatch : ActivatedAbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return ability;
                }
            }

            public static Ability ability;

            protected abstract Ability LatchAbility { get; }
            public override IEnumerator Activate()
            {
                bool flag = SceneLoader.ActiveSceneName == "Part1_Cabin";
                if (flag)
                {

                    List<CardSlot> validTargets = Singleton<BoardManager>.Instance.AllSlotsCopy;
                    validTargets.RemoveAll((CardSlot x) => x.Card == null || x.Card.Dead || CardHasLatchMod(x.Card) || x.Card == Card);



                    if (validTargets.Count <= 0)
                    {
                        yield break;
                    }


                    Singleton<ViewManager>.Instance.SwitchToView(View.Board);
                    Card.Anim.PlayHitAnimation();
                    yield return new WaitForSeconds(0.1f);


                    CardAnimationController cardAnim = Card.Anim as CardAnimationController;


                    GameObject LatchPreParent = new GameObject();

                    LatchPreParent.name = "LatchParent";
                    LatchPreParent.transform.position = cardAnim.transform.position;
                    LatchPreParent.gameObject.transform.parent = cardAnim.transform;

                    Transform Latchparent = LatchPreParent.transform;
                    GameObject claw = UnityEngine.Object.Instantiate(anthonyClawPrefab, Latchparent);


                    CardSlot selectedSlot = null;




                    if (Card.OpponentCard)
                    {
                        yield return new WaitForSeconds(0.3f);
                        yield return AISelectTarget(validTargets, delegate (CardSlot s)
                        {
                            selectedSlot = s;
                        });


                        if (selectedSlot != null && selectedSlot.Card != null)
                        {

                            Plugin.AimWeaponAnim(Latchparent.gameObject, selectedSlot.transform.position);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else
                    {
                        List<CardSlot> allSlotsCopy = Singleton<BoardManager>.Instance.AllSlotsCopy;
                        allSlotsCopy.Remove(Card.Slot);
                        yield return Singleton<BoardManager>.Instance.ChooseTarget(allSlotsCopy, validTargets, delegate (CardSlot s)
                        {
                            selectedSlot = s;
                        }, OnInvalidTarget, delegate (CardSlot s)
                        {
                            if (s.Card != null)
                            {

                                Plugin.AimWeaponAnim(Latchparent.gameObject, s.transform.position);
                            }
                        }, null, CursorType.Target);
                    }

                    claw.SetActive(true);
                    CustomCoroutine.FlickerSequence(delegate
                    {
                        claw.SetActive(true);
                    }, delegate
                    {
                        claw.SetActive(false);
                    }, startOn: true, endOn: false, 0.05f, 2);


                    if (selectedSlot != null && selectedSlot.Card != null)
                    {
                        CardModificationInfo cardModificationInfo = new CardModificationInfo(LatchAbility);


                        cardModificationInfo.fromCardMerge = true;



                        cardModificationInfo.fromLatch = true;


                        if (selectedSlot.Card.Info.name == "!DEATHCARD_BASE")
                        {
                            selectedSlot.Card.AddTemporaryMod(cardModificationInfo);
                        }
                        else
                        {

                            CardInfo targetCardInfo = selectedSlot.Card.Info.Clone() as CardInfo;

                            targetCardInfo.Mods.Add(cardModificationInfo);

                            selectedSlot.Card.SetInfo(targetCardInfo);
                        }



                        selectedSlot.Card.Anim.PlayTransformAnimation();




                        OnSuccessfullyLatched(selectedSlot.Card);
                        yield return new WaitForSeconds(0.75f);
                        yield return LearnAbility();
                    }
                }
                else
                {
                    List<CardSlot> validTargets = Singleton<BoardManager>.Instance.AllSlotsCopy;
                    validTargets.RemoveAll((CardSlot x) => x.Card == null || x.Card.Dead || CardHasLatchMod(x.Card) || x.Card == Card);
                    if (validTargets.Count <= 0)
                    {
                        yield break;
                    }
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board);
                    Card.Anim.PlayHitAnimation();
                    yield return new WaitForSeconds(0.1f);
                    DiskCardAnimationController cardAnim = Card.Anim as DiskCardAnimationController;
                    GameObject claw = UnityEngine.Object.Instantiate(anthonyClawPrefab, cardAnim.WeaponParent.transform);
                    CardSlot selectedSlot = null;
                    if (Card.OpponentCard)
                    {
                        yield return new WaitForSeconds(0.3f);
                        yield return AISelectTarget(validTargets, delegate (CardSlot s)
                        {
                            selectedSlot = s;
                        });
                        if (selectedSlot != null && selectedSlot.Card != null)
                        {
                            cardAnim.AimWeaponAnim(selectedSlot.transform.position);
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else
                    {
                        List<CardSlot> allSlotsCopy = Singleton<BoardManager>.Instance.AllSlotsCopy;
                        allSlotsCopy.Remove(Card.Slot);
                        yield return Singleton<BoardManager>.Instance.ChooseTarget(allSlotsCopy, validTargets, delegate (CardSlot s)
                        {
                            selectedSlot = s;
                        }, OnInvalidTarget, delegate (CardSlot s)
                        {
                            if (s.Card != null)
                            {
                                cardAnim.AimWeaponAnim(s.transform.position);
                            }
                        }, null, CursorType.Target);
                    }
                    CustomCoroutine.FlickerSequence(delegate
                    {
                        claw.SetActive(value: true);
                    }, delegate
                    {
                        claw.SetActive(value: false);
                    }, startOn: true, endOn: false, 0.05f, 2);
                    if (selectedSlot != null && selectedSlot.Card != null)
                    {
                        CardModificationInfo cardModificationInfo = new CardModificationInfo(LatchAbility);
                        cardModificationInfo.fromLatch = true;
                        selectedSlot.Card.Anim.ShowLatchAbility();
                        selectedSlot.Card.AddTemporaryMod(cardModificationInfo);
                        OnSuccessfullyLatched(selectedSlot.Card);
                        yield return new WaitForSeconds(0.75f);
                        yield return LearnAbility();
                    }
                }
            }

            protected virtual void OnSuccessfullyLatched(PlayableCard target)
            {
            }

            private IEnumerator AISelectTarget(List<CardSlot> validTargets, Action<CardSlot> chosenCallback)
            {
                if (validTargets.Count > 0)
                {
                    bool positiveAbility = AbilitiesUtil.GetInfo(LatchAbility).PositiveEffect;
                    validTargets.Sort((CardSlot a, CardSlot b) => AIEvaluateTarget(b.Card, positiveAbility) - AIEvaluateTarget(a.Card, positiveAbility));
                    chosenCallback(validTargets[0]);
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    Card.Anim.LightNegationEffect();
                    yield return new WaitForSeconds(0.2f);
                }
                yield break;
            }


            private int AIEvaluateTarget(PlayableCard card, bool positiveEffect)
            {
                int num = card.PowerLevel;
                if (card.Info.HasTrait(Trait.Terrain))
                {
                    num = 10 * (positiveEffect ? -1 : 1);
                }
                if (card.OpponentCard == positiveEffect)
                {
                    num += 1000;
                }
                return num;
            }

            private void OnInvalidTarget(CardSlot slot)
            {
                if (slot.Card != null && this.CardHasLatchMod(slot.Card) && !Singleton<TextDisplayer>.Instance.Displaying)
                {
                    base.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowThenClear("It's already latched...", 2.5f, 0f, Emotion.Anger, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null));
                }
            }
            private bool CardHasLatchMod(PlayableCard card)
            {
                return card.TemporaryMods.Exists((CardModificationInfo m) => m.fromLatch);
            }


        }
        public class ActivatedLatchNanoShield : Plugin.ActivatedLatch
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.ActivatedLatchNanoShield.ability;
                }
            }
            public override int EnergyCost
            {
                get
                {
                    return 1;
                }
            }
            public override int BonesCost
            {
                get
                {
                    return 2;
                }
            }
            protected override Ability LatchAbility
            {
                get
                {
                    return Ability.DeathShield;
                }
            }
            protected override void OnSuccessfullyLatched(PlayableCard target)
            {
                target.ResetShield();
                base.Card.SetCardbackSubmerged();
                base.Card.SetFaceDown(true, false);
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.SetFaceDown(false, false);
                base.Card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
                yield break;
            }
            public new static Ability ability;
        }
        public class ActivatedLatchReach : Plugin.ActivatedLatch
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.ActivatedLatchReach.ability;
                }
            }
            public override int EnergyCost
            {
                get
                {
                    return 2;
                }
            }
            protected override Ability LatchAbility
            {
                get
                {
                    return Ability.Reach;
                }
            }
            protected override void OnSuccessfullyLatched(PlayableCard target)
            {
                base.Card.SetCardbackSubmerged();
                base.Card.SetFaceDown(true, false);
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.SetFaceDown(false, false);
                base.Card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
                yield break;
            }
            public new static Ability ability;
        }
        public class ActivatedLatchBrittle : Plugin.ActivatedLatch
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.ActivatedLatchBrittle.ability;
                }
            }
            public override int EnergyCost
            {
                get
                {
                    return 1;
                }
            }
            protected override Ability LatchAbility
            {
                get
                {
                    return Ability.Brittle;
                }
            }
            protected override void OnSuccessfullyLatched(PlayableCard target)
            {
                base.Card.SetCardbackSubmerged();
                base.Card.SetFaceDown(true, false);
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.SetFaceDown(false, false);
                base.Card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
                yield break;
            }
            public new static Ability ability;
        }
        public class ActivatedLatchExplodeOnDeath : Plugin.ActivatedLatch
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.ActivatedLatchExplodeOnDeath.ability;
                }
            }
            public override int EnergyCost
            {
                get
                {
                    return 2;
                }
            }
            protected override Ability LatchAbility
            {
                get
                {
                    return Ability.ExplodeOnDeath;
                }
            }
            protected override void OnSuccessfullyLatched(PlayableCard target)
            {
                base.Card.SetCardbackSubmerged();
                base.Card.SetFaceDown(true, false);
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.SetFaceDown(false, false);
                base.Card.UpdateFaceUpOnBoardEffects();
                yield return new WaitForSeconds(0.3f);
                yield break;
            }
            public new static Ability ability;
        }
        public class IncreasePowerDecreaseHealth : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.IncreasePowerDecreaseHealth.ability;
                }
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            private void Start()
            {
                int health = base.Card.Info.Health;
                this.mod = new CardModificationInfo();
                this.mod.nonCopyable = true;
                this.mod.singletonId = "increaseATKtakeDamage";
                this.mod.attackAdjustment = 0;
                base.Card.AddTemporaryMod(this.mod);
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.TakeDamage(1, null);
                base.Card.Anim.StrongNegationEffect();
                this.mod.attackAdjustment++;
                base.Card.OnStatsChanged();
                yield return new WaitForSeconds(0.25f);
                yield return base.LearnAbility(0.25f);
                yield break;
            }
            public static Ability ability;
            private CardModificationInfo mod;
        }
        public class DecreasePowerIncreaseHealth : AbilityBehaviour
        {
            public override Ability Ability
            {
                get
                {
                    return Plugin.DecreasePowerIncreaseHealth.ability;
                }
            }
            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                return base.Card.OpponentCard != playerUpkeep;
            }
            private void Start()
            {
                int health = base.Card.Info.Health;
                this.mod = new CardModificationInfo();
                this.mod.nonCopyable = true;
                this.mod.singletonId = "increaseHPdecreaseATK";
                this.mod.healthAdjustment = 0;
                this.mod.attackAdjustment = 0;
                base.Card.AddTemporaryMod(this.mod);
            }
            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.Anim.StrongNegationEffect();
                this.mod.healthAdjustment++;
                this.mod.attackAdjustment--;
                base.Card.OnStatsChanged();
                yield return new WaitForSeconds(0.25f);
                yield return base.LearnAbility(0.25f);
                yield break;
            }
            public static Ability ability;
            private CardModificationInfo mod;
        }
    }
}
