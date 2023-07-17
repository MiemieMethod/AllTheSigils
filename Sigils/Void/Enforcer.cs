using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddEnforcer()
        {
            // setup ability
            const string rulebookName = "Enforcer";
            const string rulebookDescription = "At the start of the owner's turn, [creature] will cause adjacent creatures to attack.";
            const string LearnDialogue = "It causes it's allies to attack.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Enforcer);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Enforcer_a2);
            int powerlevel = 7;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Enforcer.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Enforcer), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Enforcer.ability] = new Tuple<string, string>("void_enforcer", "");
            }
        }
    }

    public class void_Enforcer : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }


        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> attackingSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.Slot);
            if (attackingSlots.Count > 0)
            {
                foreach (CardSlot slot in attackingSlots)
                {
                    if (slot.Card != null)
                    {
                        FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                        yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, slot);
                    }
                }
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;

        }
    }
}