using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by Blind
        private void AddResistant()
        {
            // setup ability
            const string rulebookName = "Resistant";
            const string rulebookDescription = "[creature] will only ever take 1 damage from most things. Some effects might bypass this.";
            const string LearnDialogue = "A hardy creature that one is.";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, 4, Plugin.configResistant.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("void_Resistant_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_Resistant");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_Resistant), tex);

            // set ability to behaviour class
            void_Resistant.ability = info.ability;


        }
    }

    public class void_Resistant : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }

    [HarmonyPatch(typeof(PlayableCard), "TakeDamage")]
    public class TakeDamagePatch : PlayableCard
    {
        static void Prefix(ref PlayableCard __instance, ref int damage)
        {
            if (__instance.HasAbility(void_Resistant.ability))
            {
                damage = 1;
            }
            if (__instance.HasAbility(void_ThickShell.ability))
            {
                damage--;
            }
        }
    }
}