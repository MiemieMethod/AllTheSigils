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
	[HarmonyPatch(typeof(BoardManager), "GetValueOfSacrifices")]
	public class Blood_patch
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
		[HarmonyPostfix]
		public static void Postfix(ref List<CardSlot> sacrifices, ref int __result)
		{
			int num = __result;
			foreach (CardSlot cardSlot in sacrifices)
			{
				if (cardSlot != null && cardSlot.Card != null)
				{
					if (cardSlot.Card.HasAbility(Bi_Blood.ability))
					{
						num += 1;
					} else if (cardSlot.Card.HasAbility(Quadra_Blood.ability))
					{
						num += 3;
					} 
				}
			}
			__result = num;
			return;
		}
	}
}
