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
        public void AddRight_Slash()
        {
            AbilityInfo info = AbilityManager.New(
                    OldLilyPluginGuid,
                    "Right scratch",
                    "When [creature] attacks it will also attack the space on the right of the attacked slot.",
                    typeof(Right_Slash),
                    GetTextureLily("right_scratch")
                );
            info.SetPixelAbilityIcon(GetTextureLily("right_scratch", true));
            info.powerLevel = 4;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Right_Slash.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("right_scratch", "");
            }
        }
    }

    public class Right_Slash : ExtendedAbilityBehaviour
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