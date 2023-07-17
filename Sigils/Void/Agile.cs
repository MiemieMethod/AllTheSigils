using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddAgile()
        {
            // setup ability
            const string rulebookName = "Agile";
            const string rulebookDescription = "When [creature] would be struck, it will move out of the way.";
            const string LearnDialogue = "The Card jumped out of the way to save itself...";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Agile);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Agile_a2);
            int powerlevel = 1;
            bool LeshyUsable = Plugin.configAgile.Value;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Agile.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Agile), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Agile.ability] = new Tuple<string, string>("void_agile", "");
            }
        }
    }

    public class void_Agile : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return slot == base.Card.Slot;
        }


        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool flag = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            if (flag || toRightValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.2f);
                if (toRightValid)
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toRight, 0.1f, null, true);
                }
                else
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toLeft, 0.1f, null, true);
                }
                base.Card.Anim.StrongNegationEffect();
                base.Card.RenderCard();
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
                yield return base.StartCoroutine(base.LearnAbility(0.5f));
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
}