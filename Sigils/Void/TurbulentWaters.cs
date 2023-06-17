using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void addTurbulentWaters()
        {
            // setup ability
            const string rulebookName = "Turbulent Waters";
            const string rulebookDescription = "[creature] will force other creatures with waterborne to resurface and take 1 damage at the start of the owner's turn.";
            const string LearnDialogue = "The waters be rough.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_TurbulentWaters);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_TurbulentWaters_a2);
            int powerlevel = 3;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = true;

            // set ability to behaviour class
            void_TurbulentWaters.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_TurbulentWaters), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
        }
    }

    public class void_TurbulentWaters : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return new WaitForSeconds(0.2f);
            var allslots = Singleton<BoardManager>.Instance.AllSlots;
            yield return base.PreSuccessfulTriggerSequence();
            for (var index = 0; index < allslots.Count; index++)
            {
                var slot = allslots[index];
                if (slot.Card != null && !slot.Card.HasAbility(void_TurbulentWaters.ability))
                {
                    base.Card.Anim.LightNegationEffect();
                    if (slot.Card.HasAbility(Ability.Submerge) || slot.Card.HasAbility(Ability.SubmergeSquid))
                    {
                        if (slot.Card.FaceDown)
                        {
                            yield return new WaitForSeconds(0.2f);
                            slot.Card.SetFaceDown(false, false);
                        }
                        bool impactFrameReached = false;
                        base.Card.Anim.PlayAttackAnimation(false, slot, delegate ()
                        {
                            impactFrameReached = true;
                        });
                        yield return new WaitUntil(() => impactFrameReached);
                        slot.Card.TakeDamage(1, base.Card);
                        yield return new WaitForSeconds(0.2f);
                    }
                }

            }
            yield return base.LearnAbility(0.5f);
            yield break;
        }

    }
}