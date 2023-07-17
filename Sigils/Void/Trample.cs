using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Eri
        private void AddTrample()
        {
            // setup ability
            const string rulebookName = "Trample";
            const string rulebookDescription = "When [creature] deals overkill damage to a card, the overkill damage will be sent to the owner.";
            const string LearnDialogue = "A stampede can not be stopped.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Trample);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Trample_a2);
            int powerlevel = 6;
            bool LeshyUsable = Plugin.configTrample.Value;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Trample.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Trample), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[void_Trample.ability] = new Tuple<string, string>("void_trample", "");
            }
        }
    }

    public class void_Trample : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        [HarmonyPatch(typeof(CombatPhaseManager), nameof(CombatPhaseManager.DealOverkillDamage))]
        public class TramplePatch
        {
            [HarmonyPrefix]
            public static void Prefix(ref int damage, ref CardSlot attackingSlot, ref CardSlot opposingSlot)
            {
                if (attackingSlot.Card != null && damage > 0 && attackingSlot.Card.HasAbility(void_Trample.ability))
                {
                    Singleton<LifeManager>.Instance.StartCoroutine(Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, opposingSlot.IsPlayerSlot));
                    damage = 0;
                }
            }
        }
    }
}