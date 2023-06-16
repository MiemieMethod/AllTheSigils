using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;




namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Sire
        private void AddDrawJack()
        {
            // setup ability
            const string rulebookName = "Draw Jackalope";
            const string rulebookDescription = "[creature] is played, a Jackalope is created in your hand.";
            const string LearnDialogue = "Pull a Jackalope from a hat why don't ya.";
            // const string TextureFile = "Artwork/void_pathetic.png";


            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 3);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_drawjack");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(ability_drawjack), tex);

            // set ability to behaviour class
            ability_drawjack.ability = info.ability;


        }
    }

    public class ability_drawjack : DrawCreatedCard
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override CardInfo CardToDraw
        {
            get
            {
                return CardLoader.GetCardByName("void_Jackalope");
            }
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            bool flag = Singleton<ViewManager>.Instance.CurrentView != this.DrawCardView;
            if (flag)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(this.DrawCardView, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(this.CardToDraw, base.Card.TemporaryMods, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }

    }
}