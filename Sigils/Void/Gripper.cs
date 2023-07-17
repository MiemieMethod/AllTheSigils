using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddGripper()
        {
            // setup ability
            const string rulebookName = "Gripper";
            const string rulebookDescription = "At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil, and try to move the opposing creature with it if it can.";
            const string LearnDialogue = "The trail they leave behind, hurts.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Gripper);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Gripper_a2);
            int powerlevel = 4;
            bool LeshyUsable = Plugin.configAcidTrail.Value;
            bool part1Shops = true;
            bool canStack = false;



            // set ability to behaviour class
            void_Gripper.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Gripper), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Gripper.ability] = new Tuple<string, string>("void_Gripper", "");
            }
        }
    }

    public class void_Gripper : Strafe
    {
        public override Ability Ability => ability;

        public static Ability ability;


        public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            //First check: check the card opposing the old slot. if the card is not null, there is a target to pull
            //Second check: check the new opposing slot for a card, if it is null, then we can move the card the new slot
            if (oldSlot.opposingSlot.Card != null && base.Card.slot.opposingSlot.Card == null)
            {
                yield return new WaitForSeconds(0.25f);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(oldSlot.opposingSlot.Card, base.Card.slot.opposingSlot, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
            }
            yield break;
        }
    }
}