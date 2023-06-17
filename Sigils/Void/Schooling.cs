using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;

using Art = AllTheSigils.Artwork.Resources;


namespace AllTheSigils
{
    public partial class Plugin
    {
        //Original
        private void AddSchooling()
        {
            // setup ability
            const string rulebookName = "Schooling";
            const string rulebookDescription = "[creature] will grant creatures with the waterborn 1 power.";
            const string LearnDialogue = "The waterborn stick together.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Schooling);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Schooling_a2);
            int powerlevel = 2;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Schooling.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Schooling), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
        }
    }

    public class void_Schooling : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }
}