using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Turk
        private void AddSickness()
        {
            // setup ability
            const string rulebookName = "Sickness";
            const string rulebookDescription = "[creature] will loose 1 attack each time it declares an attack.";
            const string LearnDialogue = "The creature's strength leaves it as it strikes.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -1, Plugin.configSickness.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_sick_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_sick");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_sickness), tex);

            // set ability to behaviour class
            void_sickness.ability = info.ability;


        }
    }

    public class void_sickness : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return base.Card == attacker;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (base.Card == attacker && !attacker.Dead)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.55f);
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.35f);
                CardModificationInfo cardModificationInfo = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.singletonId == "void_sickness");
                if (cardModificationInfo == null)
                {
                    cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.singletonId = "void_sickness";
                    base.Card.AddTemporaryMod(cardModificationInfo);
                }
                cardModificationInfo.attackAdjustment--;
                base.Card.OnStatsChanged();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }

    }
}