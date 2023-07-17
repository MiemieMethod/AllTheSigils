using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AllTheSigils
{
    public class SigilEffectUtils
    {
        public static bool combatPhase = false;
        public static IEnumerator moveCard(PlayableCard card, CardSlot newSlot)
        {
            Vector3 midpoint = (card.Slot.transform.position + newSlot.transform.position) / 2f;
            Tween.Position(card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(card, newSlot, 0.1f, null, true);
        }

        public static bool cardWillBeRemovedIfSacrificed(PlayableCard card)
        {
            if (card == null)
            {
                return false;
            }
            return !card.HasAbility(Resourceful.ability) && !card.HasAbility(Ability.Sacrificial) && card.CanBeSacrificed;
        }

        public static CardSlot recentlySelected;
        public static GameObject instanceTarget;
        public static GameObject target = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon");
        public static IEnumerator chooseSlot(List<CardSlot> allTargets, List<CardSlot> validTargets, Action<CardSlot> targetChosen)
        {
            if (validTargets.Count > 0)
            {
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                yield return Singleton<BoardManager>.Instance.ChooseTarget(allTargets, validTargets, CardSelected, InvalidTargetSelected, CursorEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null && SaveManager.SaveFile.IsPart1)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null)
                {
                    targetChosen(recentlySelected);
                }

                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
        }
        public static void CardSelected(CardSlot slot)
        {
            recentlySelected = slot;
        }
        public static void InvalidTargetSelected(CardSlot slot)
        {
            if (slot.Card != null)
            {
                slot.Card.Anim.StrongNegationEffect();
            }
            AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null, null, false);
        }
        public static void CursorEnteredSlot(CardSlot slot)
        {
            if (SaveManager.SaveFile.IsPart1)
            {
                if (instanceTarget != null)
                {
                    GameObject inst = instanceTarget;
                    Tween.LocalScale(inst.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(inst);
                    }, true);
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, slot.transform);
                gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                gameObject.transform.localRotation = Quaternion.identity;
                instanceTarget = gameObject;
            }
        }
    }
}
