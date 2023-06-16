using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddDwarf()
        {
            // setup ability
            const string rulebookName = "Dwarf";

            const string rulebookDescription = "When [creature] is drawn, it will loose one unit of cost, as well as 1 power and 2 health (can't go below 1 health). A unit is defined as: 1 blood, 3 bones, 3 energy, or all mox.";
            const string LearnDialogue = "What a tiny creature you have there";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 0);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Dwarf");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Dwarf), tex);

            // set ability to behaviour class
            void_Dwarf.ability = info.ability;


        }
    }

    public class void_Dwarf : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToDrawn()
        {
            return true;
        }

        public override IEnumerator OnDrawn()
        {
            (Singleton<PlayerHand>.Instance as PlayerHand3D).MoveCardAboveHand(base.Card);
            yield return base.Card.FlipInHand(new Action(this.AddMod));
            yield return base.LearnAbility(0.5f);
            yield break;
        }

        private void AddMod()
        {
            CardModificationInfo cardModificationInfo = new CardModificationInfo();
            cardModificationInfo.attackAdjustment = -1;
            int cardHealth = base.Card.MaxHealth;
            if (cardHealth > 2)
            {
                cardModificationInfo.healthAdjustment = -2;
            }
            else if (cardHealth == 2)
            {
                cardModificationInfo.healthAdjustment = -1;
            }
            cardModificationInfo.nameReplacement = string.Format(Localization.Translate("Smol {0}"), base.Card.Info.DisplayedNameLocalized);

            if (base.Card.Info.cost > 0)
            {
                cardModificationInfo.bloodCostAdjustment = -1;
            }
            else if (base.Card.Info.bonesCost > 0)
            {
                if (base.Card.Info.bonesCost >= 3)
                {
                    cardModificationInfo.bonesCostAdjustment = -3;
                }
                else if (base.Card.Info.bonesCost == 2)
                {
                    cardModificationInfo.bonesCostAdjustment = -2;
                }
                else
                {
                    cardModificationInfo.bonesCostAdjustment = -1;
                }
            }
            else if (base.Card.Info.energyCost > 0)
            {
                if (base.Card.Info.energyCost >= 3)
                {
                    cardModificationInfo.energyCostAdjustment = -3;
                }
                else if (base.Card.Info.energyCost == 2)
                {
                    cardModificationInfo.energyCostAdjustment = -2;
                }
                else
                {
                    cardModificationInfo.energyCostAdjustment = -1;

                }
            }
            else if (base.Card.Info.GemsCost.Count > 0)
            {
                cardModificationInfo.nullifyGemsCost = true;
            }
            CardInfo ClonedCardInfo = base.Card.Info.Clone() as CardInfo;

            //Add the modifincations
            ClonedCardInfo.Mods.Add(cardModificationInfo);

            for (int index = 0; index < base.Card.Info.Mods.Count; index++)
            {
                ClonedCardInfo.Mods.Add(base.Card.Info.Mods[index]);
            }


            //Update the card info
            base.Card.SetInfo(ClonedCardInfo);
        }
    }
}