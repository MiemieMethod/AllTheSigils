// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddBlood_Shifter()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Blood shifter",
                    "When a card bearing this sigils kills another card, it will turn into that card.",
                    typeof(Blood_Shifter),
                    GetTexture("blood_shifter")
                );
            info.SetPixelAbilityIcon(GetTexture("blood_shifter", true));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Blood_Shifter.ability = info.ability;
        }
    }

    public class Blood_Shifter : AbilityBehaviour
    {
        public static Ability ability;

        CardModificationInfo mod = new CardModificationInfo() { healthAdjustment = 0 };

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        private void Start()
        {
            base.Card.AddTemporaryMod(mod);
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return fromCombat && base.Card == killer && base.Card.slot.IsPlayerSlot;
        }


        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            mod.healthAdjustment = base.Card.MaxHealth - base.Card.Health;
            yield return base.Card.TransformIntoCard(card.Info, null);
            yield break;
        }
    }
}