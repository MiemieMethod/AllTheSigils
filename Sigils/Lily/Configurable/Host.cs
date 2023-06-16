// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddHost()
        {
            AbilityInfo info = AbilityManager.New(
                               OldLilyPluginGuid,
                               "Host",
                               "A card bearing this sigil is the host of other creatures. It will give you such creature when struck.",
                               typeof(Host),
                               GetTexture("host")
                           );
            info.SetPixelAbilityIcon(GetTexture("host", true));
            info.powerLevel = 5;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Host.ability = info.ability;
        }
    }

    public class Host : AbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        // Token: 0x060013ED RID: 5101 RVA: 0x0000F57E File Offset: 0x0000D77E
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000433D2 File Offset: 0x000415D2
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            if (base.Card.Info.iceCubeParams.creatureWithin != null)
            {
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(base.Card.Info.iceCubeParams.creatureWithin, new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0f, null);
            }
            else
            {
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == "RingWorm"), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            }
            yield break;
        }
    }
}