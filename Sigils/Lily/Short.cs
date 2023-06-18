// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddShort()
        {
            AbilityInfo info = AbilityManager.New(
                       OldLilyPluginGuid,
                       "Short",
                       "[creature] will not be blocked by an opposing creature bearing the airborn sigil.",
                       typeof(Short),
                       GetTexture("short")
                   );
            info.SetPixelAbilityIcon(GetTexture("short", true));
            info.powerLevel = 0;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Short.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "short";
            }
        }
    }

    public class Short : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }


        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return true;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (attacker == base.Card && slot.Card != null)
            {
                if (slot.Card.HasAbility(Ability.Flying))
                {
                    PlayableCard card = slot.Card;
                    yield return new WaitForSeconds(0.1f);
                    if (attacker.Anim != null)
                    {
                        attacker.Anim.PlayAttackAnimation(false, slot);
                    }
                    yield return new WaitForSeconds(0.25f);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(attacker.Info.Attack, attacker.Info.Attack, slot.IsPlayerSlot, 0.25f, null, 0f);
                    yield return new WaitForSeconds(0.1f);
                    card = null;
                    yield break;
                }
            }
        }
    }
}