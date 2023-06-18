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
        public void AddTribe_Attack()
        {
            AbilityInfo info = AbilityManager.New(
                      OldLilyPluginGuid,
                      "Tribe Attack",
                      "While [creature] is on the board, all other cards on your side of the board of the same tribe will gain +1 attack.",
                      typeof(Tribe_Attack),
                      GetTexture("tribe_attack")
                  );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            Tribe_Attack.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "tribe_attack";
            }
        }
    }

    public class Tribe_Attack : AbilityBehaviour
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