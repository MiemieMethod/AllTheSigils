using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Port from Cyn Sigil a day
        private void AddTransient()
        {
            // setup ability
            const string rulebookName = "Transient";
            const string rulebookDescription = "At the end of the owner's turn, [creature] will return to your hand.";
            const string LearnDialogue = "The creature blinks back into the owner's hand at the end of their turn.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("ability_transient_a2"));
            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_transient");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Transient), tex);

            // set ability to behaviour class
            void_Transient.ability = info.ability;


        }
    }

    public class void_Transient : DrawCreatedCard
    {

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public static Ability ability;

        private void Start()
        {
            this.copy = CardLoader.Clone(base.Card.Info);
        }

        public override CardInfo CardToDraw
        {
            get
            {
                return CardLoader.Clone(this.copy);
            }
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.CreateDrawnCard();
            base.Card.Anim.PlayDeathAnimation(false);
            base.Card.UnassignFromSlot();
            base.Card.StartCoroutine(base.Card.DestroyWhenStackIsClear());
            base.Card.Slot = null;
            yield break;
        }

        private CardInfo copy;
    }
}