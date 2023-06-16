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
        public void AddSong_Of_Sleep()
        {
            AbilityInfo info = AbilityManager.New(
                      OldLilyPluginGuid,
                      "Song of sleep",
                      "If a creature moves into the space opposing a card bearing this sigil, that creature will obtain the asleep sigil.",
                      typeof(Song_Of_Sleep),
                      GetTexture("song_of_sleep")
                  );
            info.SetPixelAbilityIcon(GetTexture("song_of_sleep", true));
            info.powerLevel = 2;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = false;
            info.opponentUsable = true;

            Song_Of_Sleep.ability = info.ability;
        }
    }

    public class Song_Of_Sleep : AbilityBehaviour
    {
        public static Ability ability;

        public CardModificationInfo mod = new CardModificationInfo();

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
                return -1;
            }
        }

        private void Start()
        {
            mod.abilities.Add(Asleep.ability);
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            otherCard.AddTemporaryMod(mod);
            yield break;
        }

        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
        }

        // Token: 0x06001403 RID: 5123 RVA: 0x00044398 File Offset: 0x00042598
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            otherCard.AddTemporaryMod(mod);
            yield break;
        }
    }
}