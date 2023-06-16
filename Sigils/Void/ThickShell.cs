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
        private void AddThickShell()
        {
            // setup ability
            const string rulebookName = "Thick Shell";
            const string rulebookDescription = "When attacked, [creature] takes 1 less damage.";
            const string LearnDialogue = "The thick shell on that creature protected it from one damage!";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 2, Plugin.configThickShell.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("ability_thickshell_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_thickshell");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_ThickShell), tex);

            // set ability to behaviour class
            void_ThickShell.ability = info.ability;


        }
    }

    public class void_ThickShell : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }
}
