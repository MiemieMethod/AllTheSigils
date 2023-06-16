using APIPlugin;
using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void addCoinFinder()
        {
            // setup ability
            const string rulebookName = "Coin Finder";
            const string rulebookDescription = "At the end of the owner's turn, [creature] will grant the owner 1 foil.";
            const string LearnDialogue = "A tooth for your thoughts?";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2, false);
            info.canStack = true;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_coinFinder_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_coinFinder");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_coinFinder), tex);

            // set ability to behaviour class
            void_coinFinder.ability = info.ability;



        }
    }

    public class void_coinFinder : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {

            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);
            bool flag1 = !SaveManager.SaveFile.IsPart2;
            if (flag1)
            {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("extraVoid.inscryption.LifeCost"))
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f); RunState.Run.currency += (1);
                    yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(1);
                    yield return new WaitForSeconds(0.75f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;


                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f); RunState.Run.currency += (1);
                    yield return Singleton<CurrencyBowl>.Instance.ShowGain(1, true, false);
                    yield return new WaitForSeconds(0.25f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
            }
            else
            {
                SaveData.Data.currency += 1;
                base.Card.Anim.LightNegationEffect();
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}