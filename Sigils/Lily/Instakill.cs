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
        public void AddInstakill()
        {
            AbilityInfo info = AbilityManager.New(
                 OldLilyPluginGuid,
                 "Instant",
                 "[creature] will perish immediately after its played.",
                 typeof(Instakill),
                 GetTexture("instant")
             );
            info.SetPixelAbilityIcon(GetTexture("instant", true));
            info.powerLevel = -3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Instakill.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "instant";
            }
        }
    }

    public class Instakill : AbilityBehaviour
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

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        // Token: 0x060013EE RID: 5102 RVA: 0x000441EB File Offset: 0x000423EB
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.Card.Die(false);
            yield break;
        }
    }
}