using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Art = AllTheSigils.Artwork.Resources;

using Random = UnityEngine.Random;

namespace AllTheSigils
{
    public partial class Plugin
    {
        //Request by blind
        private void AddHerd()
        {
            // setup ability
            const string rulebookName = "Herd";
            const string rulebookDescription = "[creature] will summon a copy of itself each upkeep, up to three times.";
            const string LearnDialogue = "Strength in Numbers";
            Texture2D tex_a1 = SigilUtils.LoadTextureFromResource(Art.void_Heard_3);
            Texture2D tex_a2 = SigilUtils.LoadTextureFromResource(Art.no_a2);
            int powerlevel = 4;
            bool LeshyUsable = false;
            bool part1Shops = true;
            bool canStack = false;

            // set ability to behaviour class
            void_Herd.ability = SigilUtils.CreateAbilityWithDefaultSettingsKCM(rulebookName, rulebookDescription, typeof(void_Herd), tex_a1, tex_a2, LearnDialogue,
                                                                                    true, powerlevel, LeshyUsable, part1Shops, canStack).ability;

        }
    }



    public class void_Herd : AbilityBehaviour
    {
        public override Ability Ability => ability;

        public static Ability ability;

        int herdCount = 3;




        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            if (herdCount != 0)
            {
                return base.Card.OnBoard && base.Card.OpponentCard != playerUpkeep;
            }
            else
            {
                return false;
            }
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            PlayableCard card = base.Card;
            Texture2D tex2 = SigilUtils.LoadTextureFromResource(Art.void_Heard_2);
            Texture2D tex1 = SigilUtils.LoadTextureFromResource(Art.void_Heard_1);
            Texture2D tex0 = SigilUtils.LoadTextureFromResource(Art.void_Heard_0);

            List<CardSlot> allSlots = playerUpkeep ? Singleton<BoardManager>.Instance.playerSlots : Singleton<BoardManager>.Instance.opponentSlots;
            List<CardSlot> targets = allSlots.Where(slot => slot.Card == null).ToList();

            if (targets.Count > 0)
            {
                CardSlot target = targets[Random.Range(0, targets.Count)];
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(base.Card.Info, target, 0.15f, true);

                CardModificationInfo negateMod = new CardModificationInfo();
                negateMod.negateAbilities.Add(void_Herd.ability);

                CardInfo OpponentCardInfo = target.Card.Info.Clone() as CardInfo;
                OpponentCardInfo.Mods.Add(negateMod);
                target.Card.SetInfo(OpponentCardInfo);

                yield return new WaitForSeconds(0.15f);
                yield return base.LearnAbility(0.25f);
                herdCount--;

                if (herdCount == 2)
                    base.Card.RenderInfo.OverrideAbilityIcon(void_Herd.ability, tex2);
                else if (herdCount == 1)
                    base.Card.RenderInfo.OverrideAbilityIcon(void_Herd.ability, tex1);
                else if (herdCount == 0)
                    base.Card.RenderInfo.OverrideAbilityIcon(void_Herd.ability, tex0);

                base.Card.RenderCard();
            }

            yield break;
        }
    }
}