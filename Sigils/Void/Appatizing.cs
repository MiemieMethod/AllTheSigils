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
        private void AddAppetizing()
        {
            // setup ability
            const string rulebookName = "Appetizing Target";
            const string rulebookDescription = "[creature] makes for a great target, causing the creature opposing a card bearing this sigil to gain 1 power.";
            const string LearnDialogue = "That creature makes the opponant stronger";
            // const string TextureFile = "Artwork/void_pathetic.png";

            AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, LearnDialogue, true, -2, Plugin.configAppetizing.Value);
            info.canStack = false;
            info.SetPixelAbilityIcon(SigilUtils.LoadImageAndGetTexture("no_a2"));

            Texture2D tex = SigilUtils.LoadImageAndGetTexture("void_alarm");



            AbilityManager.Add(OldVoidPluginGuid, info, typeof(void_appetizing), tex);

            // set ability to behaviour class
            void_appetizing.ability = info.ability;


        }
    }

    public class void_appetizing : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

    }

    [HarmonyPatch(typeof(PlayableCard), "GetPassiveAttackBuffs")]
    public class PortOfAlarm
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, ref PlayableCard __instance)
        {
            if (__instance.OnBoard)
            {
                if (__instance.slot.opposingSlot.Card != null && __instance.slot.opposingSlot.Card.Info.HasAbility(void_appetizing.ability))
                {
                    __result++;
                }
            }
        }
    }
}