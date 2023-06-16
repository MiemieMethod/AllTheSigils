// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddPuppets_Gift()
        {
            AbilityInfo info = AbilityManager.New(
                          OldLilyPluginGuid,
                          "Puppets gift",
                          "As long as a card bearing this sigil is on the board any cards with brittle won't die because of brittle.",
                          typeof(Puppets_Gift),
                          GetTexture("puppets_gift")
                      );
            info.SetPixelAbilityIcon(GetTexture("puppets_gift", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Puppets_Gift.ability = info.ability;
        }
    }

    public class Puppets_Gift : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            foreach (CardSlot cardslot in Singleton<BoardManager>.Instance.playerSlots)
            {
                if (cardslot.Card != null)
                {
                    if (cardslot.Card.temporaryMods != null)
                    {
                        NegateBrittle(false, cardslot.Card);
                    }
                }
            }
            yield break;
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            foreach (CardSlot cardslot in Singleton<BoardManager>.Instance.playerSlots)
            {
                if (cardslot.Card != null)
                {
                    if (cardslot.Card.temporaryMods != null)
                    {
                        NegateBrittle(true, cardslot.Card);
                    }
                }
            }
            yield break;
        }

        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            if (base.Card.OnBoard && otherCard != base.Card && otherCard.slot.IsPlayerSlot)
            {
                return true;
            }
            return false;
        }

        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            if (otherCard.temporaryMods != null)
            {
                NegateBrittle(true, otherCard);
            }
            yield break;
        }

        public void NegateBrittle(bool negate, Card card)
        {
            //create new modification info
            CardModificationInfo negateMod = new CardModificationInfo();

            //go through each of the cards default abilities and add them to the modifincation info
            negateMod.negateAbilities.Add(Ability.Brittle);

            //Clone the main card info so we don't touch the main card set
            CardInfo cardinfo = card.Info.Clone() as CardInfo;

            //Add the modifincations
            if (negate)
            {
                cardinfo.Mods.Add(negateMod);
            }
            else
            {
                cardinfo.Mods.Remove(negateMod);
            }

            //Update the opponant card info
            card.SetInfo(cardinfo);
        }
    }
}