// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddAsleep()
        {
            AbilityInfo info = AbilityManager.New(
               OldLilyPluginGuid,
               "Asleep",
               "[creature] has 0 attack for as long as it has this sigil, at the start of its owner's turn this sigil will be removed from [creature].",
               typeof(Asleep),
               GetTexture("asleep")
           );
            info.SetPixelAbilityIcon(GetTexture("asleep", true));
            info.powerLevel = -1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Asleep.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "";
            }
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilArtNames[info.ability] = "asleep";
            }
        }
    }

    public class Asleep : AbilityBehaviour
    {
        public static Ability ability;

        CardModificationInfo mod = new CardModificationInfo() { attackAdjustment = 0 };

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        private void Start()
        {
            mod.attackAdjustment = (base.Card.Attack) * -1;
            base.Card.AddTemporaryMod(mod);
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }

        // Token: 0x06001381 RID: 4993 RVA: 0x00043AA9 File Offset: 0x00041CA9
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            mod.attackAdjustment = 0;
            mod.negateAbilities.Add(Asleep.ability);
            base.Card.Status.hiddenAbilities.Add(Asleep.ability);
            yield break;
        }
    }
}