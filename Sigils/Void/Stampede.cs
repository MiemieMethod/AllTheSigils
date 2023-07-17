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
        private void AddStampede()
        {
            // setup ability
            const string rulebookName = "Stampede";
            const string rulebookDescription = "[creature] will cause adjacent creatures to attack when played on the board if played not during combat.";
            const string LearnDialogue = "Power in Numbers";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Stampede);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Stampede_a2);
            int powerlevel = 4;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Stampede.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Stampede), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Stampede.ability] = new Tuple<string, string>("void_stampede", "");
            }
        }
    }

    public class void_Stampede : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        public int DamageDealtThisPhase { get; private set; }

        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.HasAbility(void_Stampede.ability);
        }


        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> attackingSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot);
            if (attackingSlots.Count > 0)
            {
                if (!SigilEffectUtils.combatPhase)
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
            }
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;

        }
    }
}