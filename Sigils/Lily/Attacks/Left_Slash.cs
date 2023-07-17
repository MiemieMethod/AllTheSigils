// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddLeft_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Left scratch",
                    "When [creature] attacks it will also attack the space on the left of the attacked slot.",
                    typeof(Left_Slash),
                    GetTextureLily("left_scratch")
                );
            info.SetPixelAbilityIcon(GetTextureLily("left_scratch", true));
            info.powerLevel = 4;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Left_Slash.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("left_scratch", "");
            }
        }
    }

    public class Left_Slash : ExtendedAbilityBehaviour
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
            CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
            if (toLeftSlot != null)
            {
                opposingSlots.Add(toLeftSlot.opposingSlot);
            }
            else
            {
                opposingSlots.Add(base.Card.OpposingSlot());
            }
            return opposingSlots;
        }
    }
}