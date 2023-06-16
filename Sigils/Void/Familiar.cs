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
        private void AddFamiliar()
        {
            // setup ability
            const string rulebookName = "Familiar";
            const string rulebookDescription = "A familiar will help with attacking when it's adjacent allies attack a card.";
            const string LearnDialogue = "A familiar helps those in need.";
            // const string TextureFile = "Artwork/void_vicious.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 1, Plugin.configFamiliar.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_familair_a2"));
            info.flipYIfOpponent = true;
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_familair");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Familiar), tex);

            // set ability to behaviour class
            void_Familiar.ability = info.ability;


        }
    }

    public class void_Familiar : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;



        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return true;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardSlot slotSaved = base.Card.slot;
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);

            if (toLeft != null && toLeft.Card != null && toLeft.Card == attacker && !target.Dead && !target.InOpponentQueue)
            {
                yield return new WaitForSeconds(0.1f);
                yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(slotSaved, target.slot);
                yield return new WaitForSeconds(0.1f);
            }

            if (toRight != null && toRight.Card != null && toRight.Card == attacker && !target.Dead && !target.InOpponentQueue)
            {
                yield return new WaitForSeconds(0.1f);
                yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(slotSaved, target.slot);
                yield return new WaitForSeconds(0.1f);
            }
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}