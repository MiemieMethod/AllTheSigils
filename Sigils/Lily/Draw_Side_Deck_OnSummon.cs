// Using Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
// Modding Inscryption
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddDraw_Side_Deck_OnSummon()
        {
            AbilityInfo info = AbilityManager.New(
                OldLilyPluginGuid,
                "Support call",
                "When a card bearing this sigil is played, a card from your sidedeck is created in your hand.",
                typeof(Draw_Side_Deck_OnSummon),
                GetTexture("support_call")
            );
            info.SetPixelAbilityIcon(new Texture2D(17, 17));
            info.powerLevel = 3;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;

            Draw_Side_Deck_OnSummon.ability = info.ability;
        }
    }

    public class Draw_Side_Deck_OnSummon : AbilityBehaviour
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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        // Token: 0x060013EE RID: 5102 RVA: 0x000441EB File Offset: 0x000423EB
        public override IEnumerator OnResolveOnBoard()
        {
            CardInfo SideDeckCardInfo = Singleton<CardDrawPiles3D>.Instance.SideDeckData.First();
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(SideDeckCardInfo, new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            yield return new WaitForSeconds(0.45f);
            yield break;
        }
    }
}