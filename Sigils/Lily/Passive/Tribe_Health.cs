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
        public void AddTribe_Health()
        {
            AbilityInfo info = AbilityManager.New(
                         OldLilyPluginGuid,
                         "Tribe Health",
                         "While [creature] is on the board, all other cards on your side of the board of the same tribe will gain +1 health.",
                         typeof(Tribe_Health),
                         GetTexture("tribe_health")
                     );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Tribe_Health.ability = info.ability;
        }
    }

    public class Tribe_Health : AbilityBehaviour
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