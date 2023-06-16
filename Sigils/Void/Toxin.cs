using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddToxin()
        {
            // setup ability
            const string rulebookName = "Toxin";
            const string rulebookDescription = "When [creature] damages another creature, that creature looses 1 power and 1 health.";
            const string LearnDialogue = "All things can be worn down, and in different ways.";
            // const string TextureFile = "Artwork/void_weaken.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2, Plugin.configToxin.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_toxin"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_toxin");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Toxin), tex);

            // set ability to behaviour class
            void_Toxin.ability = info.ability;


        }
    }

    public class void_Toxin : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            if (target.Dead)
            {
                return false;
            }
            return true;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            if (target)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                CardModificationInfo cardModificationInfo = target.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "void_Toxin");
                if (cardModificationInfo == null)
                {
                    cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.singletonId = "void_Toxin";
                    target.AddTemporaryMod(cardModificationInfo);
                }
                cardModificationInfo.attackAdjustment--;
                cardModificationInfo.healthAdjustment--;
                target.OnStatsChanged();
                if (target.Health <= 0)
                {
                    yield return target.Die(false, base.Card, true);
                }
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            }
            yield break;
        }

    }
}