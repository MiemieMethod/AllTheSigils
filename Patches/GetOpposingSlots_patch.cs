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
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(PlayableCard), "GetOpposingSlots", 0)]
	public class GetOpposingSlots_patch
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
		[HarmonyPostfix]
		public static void Postfix(PlayableCard __instance, ref List<CardSlot> __result)
		{
			var toRightSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, false);
			var toLeftSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, true);
			if (__instance.HasAbility(Right_Slash.ability))
			{
				if (toRightSlot != null)
				{
					__result.Insert(__result.Count, toRightSlot.opposingSlot);
				} else
                {
					__result.Insert(__result.Count, __instance.Slot.opposingSlot);
				}
			}
			if (__instance.HasAbility(Left_Slash.ability))
			{
				if (toLeftSlot != null)
				{
					__result.Insert(__result.Count, toLeftSlot.opposingSlot);
				}
				else
				{
					__result.Insert(__result.Count, __instance.Slot.opposingSlot);
				}
			}
			if (__instance.HasAbility(Double_Slash.ability))
			{
				__result.Insert(__result.Count, __instance.Slot.opposingSlot);
				if (toLeftSlot != null)
				{
					__result.Insert(__result.Count, toLeftSlot.opposingSlot);
				}
				else
				{
					__result.Insert(__result.Count, __instance.Slot.opposingSlot);
				}

				if (toRightSlot != null)
				{
					__result.Insert(__result.Count, toRightSlot.opposingSlot);
				}
				else
				{
					__result.Insert(__result.Count, __instance.Slot.opposingSlot);
				}
			}
		}
	}
}
