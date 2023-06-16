using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Sire
        private void AddAntler()
        {
            // setup ability
            const string rulebookName = "Antler Bearer";
            const string rulebookDescription = "When [creature] is killed, gain three random hooved tribe cards.";
            const string LearnDialogue = "The herd sticks together.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_antler");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_antler), tex);

            // set ability to behaviour class
            void_antler.ability = info.ability;


        }
    }

    public class void_antler : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        protected virtual View DrawCardView
        {
            get
            {
                return View.Default;
            }
        }


        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }

        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {

            var cards = ScriptableObjectLoader<CardInfo>.AllData;
            List<CardInfo> targets = new List<CardInfo>();

            for (int index = 0; index < cards.Count; index++)
            {
                if (cards[index] != null && cards[index].tribes.Contains(Tribe.Hooved))
                {
                    targets.Add(cards[index]);
                }
            }


            if (Singleton<ViewManager>.Instance.CurrentView != this.DrawCardView)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(this.DrawCardView, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            var target = targets[Random.Range(0, (targets.Count))];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            target = targets[Random.Range(0, (targets.Count))];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            target = targets[Random.Range(0, (targets.Count))];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}