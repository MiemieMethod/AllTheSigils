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
        public void AddHydra()
        {
            AbilityInfo info = AbilityManager.New(
                 OldLilyPluginGuid,
                 "Exhaustion",
                 "The attack of [creature] will be decreased by the same amount as its lost health.",
                 typeof(Hydra),
                 GetTexture("exhaustion")
             );
            info.SetPixelAbilityIcon(GetTexture("exhaustion", true));
            info.powerLevel = -2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Hydra.ability = info.ability;
        }
    }

    public class Hydra : AbilityBehaviour
    {
        public CardModificationInfo mod = new CardModificationInfo();
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        private void Start()
        {
            mod.attackAdjustment = 0;
            base.Card.AddTemporaryMod(mod);
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            mod.attackAdjustment = 0;
            yield break;
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            if (target == base.Card)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            int losthealth = ((base.Card.Info.baseHealth - base.Card.Info.Health) * -1) - amount;
            if (losthealth < base.Card.Info.baseAttack)
            {
                if (losthealth < 0)
                {
                    mod.attackAdjustment = losthealth;
                }
                else
                {
                    mod.attackAdjustment = 0;
                }
            }
            else
            {
                mod.attackAdjustment = (base.Card.Info.baseAttack * -1);
            }
            yield break;
        }
    }
}