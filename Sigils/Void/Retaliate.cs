using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddRetaliate()
        {
            // setup ability
            const string rulebookName = "Retaliate";
            const string rulebookDescription = "[creature] will strike those who strike their adjacent allies.";
            const string LearnDialogue = "It will defend it's allies";
            // const string TextureFile = "Artwork/void_vicious.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 5, Plugin.configFamiliar.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_Retaliate_a2"));
            info.flipYIfOpponent = true;
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Retaliate");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Retaliate), tex);

            // set ability to behaviour class
            void_Retaliate.ability = info.ability;


        }
    }

    public class void_Retaliate : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;



        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            return !attacker.Dead && target.slot == toLeft || target.slot == toRight;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return new WaitForSeconds(0.25f);
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.25f);
            CardModificationInfo removeFlyingMod = null;
            bool flag = base.Card.HasAbility(Ability.Flying);
            if (flag)
            {
                removeFlyingMod = new CardModificationInfo();
                removeFlyingMod.negateAbilities.Add(Ability.Flying);
                base.Card.AddTemporaryMod(removeFlyingMod);
            }
            yield return Singleton<TurnManager>.Instance.CombatPhaseManager.SlotAttackSlot(base.Card.Slot, attacker.Slot, 0f);
            bool flag2 = removeFlyingMod != null;
            if (flag2)
            {
                base.Card.RemoveTemporaryMod(removeFlyingMod, true);
            }
            yield return new WaitForSeconds(0.25f);
            yield break;
        }
    }
}