using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        private void AddTakeAllNegatives()
        {
            // setup ability
            const string rulebookName = "Disease Absorbtion";
            const string rulebookDescription = "When played, [creature] will take all negative sigils onto itself.";
            const string LearnDialogue = "How Noble.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 0);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_takeDisease_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_takeDisease");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(ability_TakeAllNegatives), tex);

            // set ability to behaviour class
            ability_TakeAllNegatives.ability = info.ability;


        }
    }

    public class ability_TakeAllNegatives : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);

            PlayableCard crows = (PlayableCard)base.Card;
            var PLCards = Singleton<BoardManager>.Instance.GetSlots(true);


            crows.Anim.StrongNegationEffect();
            crows.Anim.PlaySacrificeParticles();

            var abilities = ScriptableObjectLoader<AbilityInfo>.AllData;


            for (int i = 0; i < PLCards.Count; i++)
            {
                if (PLCards[i].Card != null && PLCards[i].Card != base.Card)
                {
                    PlayableCard target = PLCards[i].Card;
                    target.Anim.StrongNegationEffect();
                    target.Anim.PlaySacrificeParticles();


                    for (int index = 0; index < abilities.Count; index++)
                    {
                        if (target.HasAbility(abilities[index].ability) && abilities[index].powerLevel < 0)
                        {
                            yield return new WaitForSeconds(0.2f);
                            //create new modification info
                            CardModificationInfo negateMod = new CardModificationInfo();
                            negateMod.negateAbilities.Add(abilities[index].ability);
                            CardInfo cardInfo = target.Info.Clone() as CardInfo;
                            cardInfo.Mods.Add(negateMod);
                            target.SetInfo(cardInfo);
                            target.Anim.LightNegationEffect();

                            CardModificationInfo negativeAbilityMod = new CardModificationInfo(abilities[index].ability);
                            crows.AddTemporaryMod(negativeAbilityMod);
                            crows.Anim.LightNegationEffect();

                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            yield break;
        }
    }
}