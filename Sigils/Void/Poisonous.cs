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
        private void AddPoisonous()
        {
            // setup ability
            const string rulebookName = "Poisonous";
            const string rulebookDescription = "When [creature] perishes, the creature that killed it perishes as well.";
            const string LearnDialogue = "Attacking something poisonous, isn't that smart.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("ability_poisonous_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_poisonous");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Poisonous), tex);

            // set ability to behaviour class
            void_Poisonous.ability = info.ability;


        }
    }

    public class void_Poisonous : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && base.Card.OnBoard;
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.25f);
            if (killer != null)
            {
                yield return killer.Die(false, base.Card, true);
                if (Singleton<BoardManager>.Instance is BoardManager3D)
                {
                    yield return new WaitForSeconds(0.5f);
                    yield return base.LearnAbility(0.5f);
                }
            }
            yield break;
        }
    }
}
