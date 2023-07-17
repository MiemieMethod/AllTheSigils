// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddAsleep()
        {
            AbilityInfo info = AbilityManager.New(
               OldLilyPluginGuid,
               "Asleep",
               "[creature] has 0 attack for as long as it has this sigil, at the start of the player's turn this sigil will be removed from [creature].",
               typeof(Asleep),
               GetTextureLily("asleep")
           );
            info.SetPixelAbilityIcon(GetTextureLily("asleep", true));
            info.powerLevel = -1;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Asleep.ability = info.ability;
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("", "");
            }
            if (Plugin.GenerateWiki)
            {
                Plugin.SigilWikiInfos[info.ability] = new Tuple<string, string>("asleep", "");
            }
        }
    }

    public class Asleep : AbilityBehaviour
    {
        public static Ability ability;

        CardModificationInfo mod = new CardModificationInfo() { attackAdjustment = 0 };

        float amountOfTurnsTillRemoval;

        int amountOfStacksOnCard;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        private void Start()
        {
            List<Ability> AllAbilities = base.Card.Info.Abilities;
            foreach (List<Ability> abilityList in base.Card.TemporaryMods.Select(x => x.abilities).ToList())
            {
                AllAbilities.AddRange(abilityList);
            }
            amountOfStacksOnCard = AllAbilities.Where(x => x == Asleep.ability).ToList().Count;
            amountOfTurnsTillRemoval = amountOfStacksOnCard;
            Plugin.Log.LogInfo(amountOfStacksOnCard);
            mod.attackAdjustment = (base.Card.Attack) * -1;
            base.Card.AddTemporaryMod(mod);
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            //weird way of doing it but i couldn't think of a better way
            amountOfTurnsTillRemoval -= 1 / amountOfStacksOnCard;

            if (amountOfTurnsTillRemoval <= 0)
            {
                mod.attackAdjustment = 0;
                mod.negateAbilities.Add(Asleep.ability);
                base.Card.Status.hiddenAbilities.Add(Asleep.ability);
            }
            yield break;
        }
    }
}