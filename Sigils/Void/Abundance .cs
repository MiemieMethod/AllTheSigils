using APIPlugin;
using DiskCardGame;
using GBC;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;



namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by blind
        private void AddAbundance()
        {
            // setup ability
            const string rulebookName = "Abundance";
            const string rulebookDescription = "[creature] will grant one tooth per instance of Abundance when killed.";
            const string LearnDialogue = "Gooooooooldddddd! *cough* sorry about that. Couldn't resist.";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Abundance);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.void_Abundance_a2);
            int powerlevel = 1;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;



            // set ability to behaviour class
            void_Abundance.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Abundance), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[void_Abundance.ability] = "void_Abundance";
            }
        }
    }

    public class void_Abundance : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return base.Card.HasAbility(void_Abundance.ability);
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {

            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.15f);

            bool flag2 = !SaveManager.SaveFile.IsPart2;
            if (flag2)
            {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("extraVoid.inscryption.LifeCost"))
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f); RunState.Run.currency += (1);
                    yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(1);
                    yield return new WaitForSeconds(0.75f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, true);
                    yield return new WaitForSeconds(0.25f); RunState.Run.currency += (1);
                    yield return Singleton<CurrencyBowl>.Instance.ShowGain(1, true, false);
                    yield return new WaitForSeconds(0.25f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
            }
            else
            {
                SaveData.Data.currency += 1;
                base.Card.Anim.LightNegationEffect();
            }

            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}