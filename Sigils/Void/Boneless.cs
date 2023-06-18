using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddBoneless()
        {
            // setup ability
            const string rulebookName = "Boneless";
            const string rulebookDescription = "[creature] gives no bones! Any bones gained from sigils or death will be negated.";
            const string LearnDialogue = "That creature has no bones!";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Boneless);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Boneless_a2);
            int powerlevel = -2;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Boneless.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Boneless), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Boneless.ability] = "void_Boneless";
            }
        }
    }

    public class void_Boneless : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;


        [HarmonyPatch(typeof(ResourcesManager), nameof(ResourcesManager.AddBones))]
        public class Boneless_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(int amount, CardSlot slot)
            {
                if (slot != null
                && slot.Card != null
                && slot.Card.HasAbility(void_Boneless.ability))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}