// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddDouble_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                             OldLilyPluginGuid,
                             "Double scratch",
                             "When [creature] attacks it attacks twice and the space right and left of the attacked slot.",
                             typeof(Double_Slash),
                             GetTextureLily("double_scratch")
                         );
            info.SetPixelAbilityIcon(GetTextureLily("double_scratch", true));
            info.powerLevel = 6;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Double_Slash.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("double_scratch", "");
            }
        }
    }

    public class Double_Slash : ExtendedAbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToGetOpposingSlots()
        {
            return true;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> opposingSlots = new List<CardSlot>();
            opposingSlots.Add(base.Card.OpposingSlot());

            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            if (toLeftSlot != null)
            {
                opposingSlots.Add(toLeftSlot.opposingSlot);
            }
            else
            {
                opposingSlots.Add(base.Card.OpposingSlot());
            }

            CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
            if (toRightSlot != null)
            {
                opposingSlots.Add(toRightSlot.opposingSlot);
            }
            else
            {
                opposingSlots.Add(base.Card.OpposingSlot());
            }
            return opposingSlots;
        }
    }
}