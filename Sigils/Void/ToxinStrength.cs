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
        private void AddToxinStrength()
        {
            // setup ability
            const string rulebookName = "Toxin (Strength)";
            const string rulebookDescription = "When [creature] damages another creature, that creature looses 1 power.";
            const string LearnDialogue = "Even once combat is over, strength leaves it's target";
            // const string TextureFile = "Artwork/void_weaken.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 1, Plugin.configToxin.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_toxin_strength_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_toxin_strength");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_ToxinStrength), tex);

            // set ability to behaviour class
            void_ToxinStrength.ability = info.ability;


        }
    }

    public class void_ToxinStrength : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        private CardModificationInfo mod;

        private void Start()
        {
            this.mod = new CardModificationInfo();
            this.mod.attackAdjustment = -1;
        }

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
            if (target != null)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                CardModificationInfo cardModificationInfo = target.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "void_ToxinStrength");
                if (cardModificationInfo == null)
                {
                    cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.singletonId = "void_ToxinStrength";
                    target.AddTemporaryMod(cardModificationInfo);
                }
                cardModificationInfo.attackAdjustment--;
                target.OnStatsChanged();
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            }
            yield break;
        }

    }
}