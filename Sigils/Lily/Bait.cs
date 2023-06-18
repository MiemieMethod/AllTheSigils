// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddBait()
        {
            AbilityInfo info = AbilityManager.New(
                 OldLilyPluginGuid,
                 "Bait",
                 "When an opposing creature is played and there is no card opposite of [creature], the opposing creature will move to that spot.",
                 typeof(Bait),
                 GetTexture("bait")
             );
            info.SetPixelAbilityIcon(GetTexture("bait", true));
            info.powerLevel = 1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Bait.ability = info.ability;
        }
    }

    public class Bait : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        // Token: 0x060013C1 RID: 5057 RVA: 0x00043EC8 File Offset: 0x000420C8
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard != null && otherCard.Slot != null && ((otherCard.Slot.IsPlayerSlot && !base.Card.Slot.IsPlayerSlot) || (!otherCard.Slot.IsPlayerSlot && base.Card.Slot.IsPlayerSlot)) && otherCard.Slot != base.Card.Slot.opposingSlot;
        }

        // Token: 0x060013C2 RID: 5058 RVA: 0x00043F4F File Offset: 0x0004214F
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);
            CardSlot targetSlot = base.Card.Slot.opposingSlot;
            if (targetSlot.Card != null)
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                Vector3 a = (otherCard.Slot.transform.position + targetSlot.transform.position) / 2f;
                Tween.Position(otherCard.transform, a + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(otherCard, targetSlot, 0.1f, null, true);
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}