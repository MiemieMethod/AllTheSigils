// Using Inscryption
using DiskCardGame;

// Modding Inscryption
using InscryptionAPI.Card;

using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddShape_Shifter()
        {
            AbilityInfo info = AbilityManager.New(
                               OldLilyPluginGuid,
                               "Shapeshifter",
                               "A card bearing this sigil is ever changing. It will change its form once it's struck.",
                               typeof(Shape_Shifter),
                               GetTexture("shape_shifter")
                           );
            info.SetPixelAbilityIcon(GetTexture("shape_shifter", true));
            info.powerLevel = 5;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Shape_Shifter.ability = info.ability;
        }
    }

    public class Shape_Shifter : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override int Priority
        {
            get
            {
                return 1;
            }
        }

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            if (source.Attack < base.Card.Health)
            {
                return true;
            }
            return false;
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000433D2 File Offset: 0x000415D2
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (base.Card.Info.iceCubeParams.creatureWithin != null)
            {
                yield return base.Card.TransformIntoCard(base.Card.Info.iceCubeParams.creatureWithin, null);
            }
            else
            {
                yield return base.Card.TransformIntoCard(CardLoader.AllData.Find(info => info.name == "Amoeba"), null);
            }
        }
    }
}