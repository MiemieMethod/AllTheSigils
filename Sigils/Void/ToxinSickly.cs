using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Turk
        private void AddToxinSickly()
        {
            // setup ability
            const string rulebookName = "Toxin (Sickening)";
            const string rulebookDescription = "When [creature] damages another creature, that creature gains the Sickness Sigil. The Sickness Sigil is defined as: When ever a creature bearing this sigil declares an attack, they will loose one attack.";
            const string LearnDialogue = "Even once combat is over, it leaves sickness spreads in what it fights.";
            // const string TextureFile = "Artwork/void_weaken.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2, Plugin.configToxin.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_toxin_sickness_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_toxin_sickness");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_ToxinSickly), tex);

            // set ability to behaviour class
            void_ToxinSickly.ability = info.ability;


        }
    }

    public class void_ToxinSickly : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            if (target.Dead)
            {
                return false;
            }
            return base.Card.HasAbility(void_ToxinSickly.ability);
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            if (target != null)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                //make the card mondification info
                CardModificationInfo cardModificationInfo = new CardModificationInfo(void_sickness.ability);
                //Clone the main card info so we don't touch the main card set
                CardInfo targetCardInfo = target.Info.Clone() as CardInfo;
                //Add the modifincations to the cloned info
                targetCardInfo.Mods.Add(cardModificationInfo);
                //Set the target's info to the clone'd info
                target.SetInfo(targetCardInfo);
                target.Anim.PlayTransformAnimation();
                Plugin.Log.LogWarning("toxin debug " + target + " should have sickness");
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            }
            yield break;
        }
    }
}