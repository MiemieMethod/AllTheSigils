using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



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
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Antler);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.no_a2);
            int powerlevel = 2;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Antler.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Antler), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Antler.ability] = "void_antler";
            }


        }
    }

    public class void_Antler : AbilityBehaviour
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

            var target = targets[SeededRandom.Range(0, (targets.Count), base.GetRandomSeed() + 1)];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            target = targets[SeededRandom.Range(0, (targets.Count), base.GetRandomSeed() + 2)];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            target = targets[SeededRandom.Range(0, (targets.Count), base.GetRandomSeed() + 3)];

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(target, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);

            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}