using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils.Voids_work.Cards
{
    public static class Jackalope
    {
        public static void AddCard()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<Tribe> Tribes = new List<Tribe>() { Tribe.Hooved };

            List<Ability> Abilities = new List<Ability>() {
                Ability.Strafe,
                Plugin.GetCustomAbility("extraVoid.inscryption.voidSigils", "Draw Jackalope")
            };

            Texture2D DefaultTexture = SigilUtils.LoadImageAndGetTexture("void_Jack");
            Texture2D pixelTexture = SigilUtils.LoadImageAndGetTexture("void_Caustic_Puddle_p");

            Texture2D eTexture = SigilUtils.LoadImageAndGetTexture("void_Jack_e");

            CardInfo info = new CardInfo()
            {
                name = "Jackalope",
                displayedName = "Jackalope",
                baseAttack = 2,
                baseHealth = 2,
                metaCategories = metaCategories,
                cardComplexity = CardComplexity.Advanced,
                temple = CardTemple.Nature,
                description = "Jacka",
                cost = 2,
                tribes = Tribes,
                abilities = Abilities
            };
            info.SetPortrait(DefaultTexture);
            info.SetEmissivePortrait(eTexture);
            info.SetPixelPortrait(pixelTexture);

            CardManager.Add("void", info);
        }
    }
}

