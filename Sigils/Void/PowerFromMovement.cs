using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Sire
        private void AddMovingPowerUp()
        {
            // setup ability
            const string rulebookName = "Power from Movement";
            const string rulebookDescription = "At the start of the owner's turn, [creature] will gain 1 power and 1 health if it moved last round.";
            const string LearnDialogue = "Each move, it grows";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 1);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_MovementPowerUp");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_MovingPowerUp), tex);

            // set ability to behaviour class
            void_MovingPowerUp.ability = info.ability;


        }
    }

    public class void_MovingPowerUp : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public static CardSlot lastSlot = null;

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            lastSlot = base.Card.Slot;
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (lastSlot == base.Card.Slot)
            {
                yield break;
            }
            CardModificationInfo cardModificationInfo = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "void_MovingPowerUp");
            if (cardModificationInfo == null)
            {
                cardModificationInfo = new CardModificationInfo();
                cardModificationInfo.singletonId = "void_MovingPowerUp";
                base.Card.AddTemporaryMod(cardModificationInfo);
            }
            cardModificationInfo.attackAdjustment++;
            cardModificationInfo.healthAdjustment++;
            base.Card.OnStatsChanged();
            yield return new WaitForSeconds(0.25f);
            lastSlot = base.Card.Slot;
            yield break;
        }
    }
}