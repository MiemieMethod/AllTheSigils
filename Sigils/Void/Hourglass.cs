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
        //Request by Sire
        private void AddHourglass()
        {
            // setup ability
            const string rulebookName = "Hourglass";
            const string rulebookDescription = "[creature] will cause the opponant to skip their turn when played.";
            const string LearnDialogue = "The sands of time tic away";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 6);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Hourglass");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Hourglass), tex);

            // set ability to behaviour class
            void_Hourglass.ability = info.ability;


        }
    }

    public class void_Hourglass : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.2f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield return new WaitForSeconds(0.25f);
            Singleton<TurnManager>.Instance.Opponent.SkipNextTurn = true;
            Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("I'll pass my next turn.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
            yield return new WaitForSeconds(0.2f);
            yield return base.LearnAbility(0f);
            yield break;
        }


    }
}