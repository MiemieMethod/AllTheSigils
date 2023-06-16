// Using Inscryption
using UnityEngine;
using DiskCardGame;

// Modding Inscryption
using InscryptionAPI.Card;

using System.Collections;
using System.Collections.Generic;
using System.IO; // Loading Sigil and Card art.
using System.Linq;
using HarmonyLib;

namespace AllTheSigils
{
	// Token: 0x02000006 RID: 6Picky
	[HarmonyPatch(typeof(PlayableCard), "CanPlay")]
	public class CanPlay_patch
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
		[HarmonyPostfix]
		public static void Postfix(ref PlayableCard __instance, ref bool __result)
		{
			if (__instance.Info.HasAbility(Picky.ability) && __instance.Info.BloodCost > 0)
            {
				List<CardSlot> playerslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
				int AvailableSacrificeValue = Singleton<BoardManager>.Instance.GetValueOfSacrifices(playerslots.FindAll((CardSlot x) => x.Card != null && x.Card.CanBeSacrificed && (x.Card.Info.BloodCost != 0 || x.Card.Info.BonesCost != 0 || x.Card.Info.EnergyCost != 0)));
				foreach (CardSlot cardslot in playerslots)
                {
					if (cardslot.Card != null)
					{
						Plugin.Log.LogInfo(cardslot.Card.Info.name);
					}
                }
				__result = __instance.Info.BloodCost <= AvailableSacrificeValue;
			}
			return;
		}
	}
}
