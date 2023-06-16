// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddLinguist()
        {
            AbilityInfo info = AbilityManager.New(
                       OldLilyPluginGuid,
                       "Linguist",
                       "While a card bearing this sigil is on the board, all talking cards on your side of the board get +1 attack.",
                       typeof(Linguist),
                       GetTexture("linguist")
                   );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Linguist.ability = info.ability;
        }
    }

    public class Linguist : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}