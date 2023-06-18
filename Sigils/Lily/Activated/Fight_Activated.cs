// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddFight_Activated()
        {
            AbilityInfo info = AbilityManager.New(
               OldLilyPluginGuid,
               "Charge",
               "Pay 3 bones to choose an enemy creature that [creature] will strike.",
               typeof(Fight_Activated),
               GetTexture("charge")
           );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 5;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;
            info.activated = true;

            Fight_Activated.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "charge";
            }
        }
    }

    public class Fight_Activated : ActivatedAbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override int BonesCost
        {
            get
            {
                return 3;
            }
        }


        public override IEnumerator Activate()
        {
            List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
            List<CardSlot> validtargets = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
            validtargets.RemoveAll((CardSlot x) => x.Card == null);
            if (validtargets.Count != 0)
            {
                CardSlot slot = base.Card.slot;
                List<CardSlot> opposingSlots = new List<CardSlot>();
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                CardSlot cardSlot = Singleton<InteractionCursor>.Instance.CurrentInteractable as CardSlot;
                BoardManager instance = Singleton<BoardManager>.Instance;
                CardSlot target = null;
                yield return instance.ChooseTarget(opponentSlotsCopy, validtargets, delegate (CardSlot slot) { target = slot; }, null, null, () => false, CursorType.Target);
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(slot, target, 0.1f);
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.DefaultView, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            else
            {
                yield return Singleton<ResourcesManager>.Instance.AddBones(3, base.Card.Slot);
            }
            yield break;
        }
    }
}