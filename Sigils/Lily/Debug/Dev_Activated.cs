// Using Inscryption
// Modding Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllTheSigils
{
    public partial class Plugin
    {
        public void AddDev_Activated()
        {
            AbilityInfo info = AbilityManager.New(
                          OldLilyPluginGuid,
                          "Wharrgarbl",
                          "Wharrgarbl.",
                          typeof(Dev_Activated),
                          GetTextureLily("placeholder")
                      );
            info.SetPixelAbilityIcon(GetTextureLily("placeholder", true));
            info.powerLevel = 69420;
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = false;
            info.activated = true;

            Dev_Activated.ability = info.ability;
        }
    }

    public class Dev_Activated : ActivatedAbilityBehaviour
    {
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override int BonesCost
        {
            get
            {
                return 0;
            }
        }

        public override IEnumerator Activate()
        {
            //PlayableCard card = base.Card.OpposingSlot()?.Card;
            //if (card != null)
            //{
            //    CardModificationInfo mod = new CardModificationInfo();
            //    mod.abilities = new List<Ability>() { Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp, Ability.Sharp };
            //    card.AddTemporaryMod(mod);
            //}
            string cardname = "Wolf";
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            //yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            string cardname2 = "Wolf_Talking";
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname2), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            string cardname3 = "Stoat";
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname3), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            string cardname4 = "Grizzly";
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname4), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            string cardname5 = "Mole";
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname5), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            //string cardname5 = "Squirrel";
            //yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.AllData.Find(info => info.name == cardname5), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
            yield break;
        }
    }
}