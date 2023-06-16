using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddProtector()
        {
            // setup ability
            const string rulebookName = "Protector";
            const string rulebookDescription = "[creature] will attacks on adjacent allies to hit directly.";
            const string LearnDialogue = "They protect their allies, but who protects you?";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_protector_a2"));
            info.metaCategories.Remove(AbilityMetaCategory.Part1Modular);

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_protector");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Protector), tex);

            // set ability to behaviour class
            void_Protector.ability = info.ability;




        }
    }

    public class void_Protector : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return true;
        }


        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            PlayableCard protector = base.Card;
            List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(protector.slot);
            if (adjacentSlots.Count > 0 && adjacentSlots[0].Index < protector.slot.Index)
            {
                if (adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                {
                    if (adjacentSlots[0] == slot)
                    {
                        yield return new WaitForSeconds(0.1f);
                        if (attacker.Anim is CardAnimationController)
                        {
                            (attacker.Anim as CardAnimationController).PlayAttackAnimation(false, slot);

                        }
                        yield return new WaitForSeconds(0.25f);
                        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(attacker.Info.Attack, attacker.Info.Attack, slot.IsPlayerSlot, 0.25f, null, 0f);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                adjacentSlots.RemoveAt(0);
            }
            if (adjacentSlots.Count > 0 && adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
            {
                if (adjacentSlots[0] == slot)
                {
                    yield return new WaitForSeconds(0.1f);
                    if (attacker.Anim is CardAnimationController)
                    {
                        (attacker.Anim as CardAnimationController).PlayAttackAnimation(false, slot);

                    }
                    yield return new WaitForSeconds(0.25f);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(attacker.Info.Attack, attacker.Info.Attack, slot.IsPlayerSlot, 0.25f, null, 0f);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield break;
        }


    }

    [HarmonyPatch(typeof(PlayableCard), "AttackIsBlocked", MethodType.Normal)]
    public class AttackIsBlocked_Protector_Patch
    {

        [HarmonyPostfix]
        public static void Postfix(ref CardSlot opposingSlot, ref bool __result)
        {

            if (opposingSlot.Card != null)
            {
                Plugin.Log.LogDebug("protector test succesfully patched");
                List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(opposingSlot);

                if (adjacentSlots.Count > 0 && adjacentSlots[0].Index < opposingSlot.Index)
                {
                    if (adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                    {
                        Plugin.Log.LogDebug("protector test: card: " + adjacentSlots[0].Card);
                        Plugin.Log.LogDebug("protector test: ability: " + adjacentSlots[0].Card.HasAbility(void_Protector.ability));
                        if (adjacentSlots[0].Card.HasAbility(void_Protector.ability))
                        {
                            __result = true;
                        }
                    }
                    adjacentSlots.RemoveAt(0);
                }
                if (adjacentSlots.Count > 0 && adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                {
                    Plugin.Log.LogDebug("protector test: card: " + adjacentSlots[0].Card);
                    Plugin.Log.LogDebug("protector test: ability: " + adjacentSlots[0].Card.HasAbility(void_Protector.ability));
                    if (adjacentSlots[0].Card.HasAbility(void_Protector.ability))
                    {
                        __result = true;
                    }
                }
                Plugin.Log.LogDebug(__result);
            }
        }
    }
}