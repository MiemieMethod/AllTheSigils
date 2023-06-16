// Using Inscryption
using UnityEngine;
using DiskCardGame;

// Modding Inscryption
using InscryptionAPI.Card;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; // Loading Sigil and Card art.
using System.Linq;
using HarmonyLib;
using Random = System.Random;

namespace AllTheSigils
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(BoardManager), "ChooseSacrificesForCard", 0)]
	public class ChooseSacrifices_patch
	{
		public static List<Tribe> tribes = new List<Tribe>();

		// Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
		[HarmonyPrefix]
		public static bool Prefix(ref List<CardSlot> validSlots, ref PlayableCard card)
		{
			if (card.HasAbility(Picky.ability))
			{
				for (int validslotindex = 0; validslotindex < validSlots.Count; validslotindex++)
				{
					if (validSlots[validslotindex].Card.Info.BloodCost == 0 && validSlots[validslotindex].Card.Info.BonesCost == 0 && validSlots[validslotindex].Card.Info.EnergyCost == 0)
					{
						validSlots.RemoveAt(validslotindex);
					}
				}
			}
			return true;
		}
	}
}
