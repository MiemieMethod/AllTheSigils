using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddVicious()
        {
            // setup ability
            const string rulebookName = "Vicious";
            const string rulebookDescription = "When [creature] is attacked, it gains 1 power.";
            const string LearnDialogue = "A hit just makes it angry.";
            // const string TextureFile = "Artwork/void_vicious.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 1, Plugin.configVicious.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_vicious_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_vicious");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Vicious), tex);

            // set ability to behaviour class
            void_Vicious.ability = info.ability;


        }
    }

    public class void_Vicious : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        private CardModificationInfo mod;

        private void Start()
        {
            this.mod = new CardModificationInfo();
            this.mod.attackAdjustment = 1;
        }



        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return base.Card.OnBoard;
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (source)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.AddTemporaryMod(this.mod);
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            }
            yield break;
        }
    }
}