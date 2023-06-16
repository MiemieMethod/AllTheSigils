// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddImbuing()
        {
            AbilityInfo info = AbilityManager.New(
                 OldLilyPluginGuid,
                 "Imbuing",
                 "A card bearing this sigil will get specific buffs depending on which tribe is most promenent in the sacrifices that were used to summon the card.",
                 typeof(Imbuing),
                 GetTexture("imbuing")
             );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = false;

            Imbuing.ability = info.ability;
        }
    }

    public class Imbuing : AbilityBehaviour
    {
        public static Dictionary<Tribe, CardModificationInfo> SpecialEffects = new Dictionary<Tribe, CardModificationInfo>(){
            {Tribe.Canine, new CardModificationInfo(Ability.GuardDog) {attackAdjustment=1, healthAdjustment=1 } },
            {Tribe.Bird, new CardModificationInfo(Ability.Flying) {attackAdjustment=1} },
            {Tribe.Reptile, new CardModificationInfo(Ability.TailOnHit) {healthAdjustment=1} },
            {Tribe.Insect, new CardModificationInfo(Tribe_Attack.ability) {} },
            {Tribe.Squirrel, new CardModificationInfo(Bi_Blood.ability) {} },
            {Tribe.Hooved, new CardModificationInfo(Ability.SplitStrike) {} }
        };

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        // Token: 0x060013EE RID: 5102 RVA: 0x000441EB File Offset: 0x000423EB
        public override IEnumerator OnResolveOnBoard()
        {
            List<Tribe> tribes = new List<Tribe>();
            foreach (CardInfo cardinfo in Singleton<BoardManager>.Instance.LastSacrificesInfo)
            {
                if (cardinfo.tribes.Count > 0)
                {
                    cardinfo.tribes.ForEach(x => tribes.Add(x));
                }
            }

            if (tribes.Count > 0)
            {
                var most = (from i in tribes
                            group i by i into grp
                            orderby grp.Count() descending
                            select new { grp.Key, Count = grp.Count() });

                List<Tribe> result = most.SelectMany(a => Enumerable.Repeat(a.Key, a.Count)).ToList();
                Random random = new Random();
                CardModificationInfo mod = Imbuing.SpecialEffects[result[random.Next(result.Count)]];
                base.Card.AddTemporaryMod(mod);
                base.Card.Status.hiddenAbilities.Add(Imbuing.ability);
                base.Card.Info.Abilities.Remove(Imbuing.ability);
            }
            yield break;
        }
    }
}