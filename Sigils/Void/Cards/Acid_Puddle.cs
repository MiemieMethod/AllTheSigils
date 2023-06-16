using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections.Generic;
using UnityEngine;



namespace AllTheSigils.Voids_work.Cards
{
    public static class Acid_Puddle
    {
        public static void AddCard()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<Ability> Abilities = new List<Ability>() {
                Ability.Sharp
            };

            List<Trait> Traits = new List<Trait>() {
                Trait.Terrain
            };

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>() {
                CardAppearanceBehaviour.Appearance.TerrainBackground
            };

            Texture2D DefaultTexture = SigilUtils.LoadImageAndGetTexture("void_Caustic_Puddle");
            Texture2D pixelTexture = SigilUtils.LoadImageAndGetTexture("void_Caustic_Puddle_p");

            Texture2D eTexture = SigilUtils.LoadImageAndGetTexture("void_Caustic_Puddle_e");

            CardInfo info = new CardInfo()
            {
                name = "Acid_Puddle",
                displayedName = "Acid Puddle",
                baseAttack = 0,
                baseHealth = 1,
                metaCategories = metaCategories,
                cardComplexity = CardComplexity.Advanced,
                temple = CardTemple.Nature,
                description = "A puddle of Acid, dangerous to the touch",
                traits = Traits,
                abilities = Abilities,
                appearanceBehaviour = appearanceBehaviour,
            };
            info.SetPortrait(DefaultTexture);
            info.SetEmissivePortrait(eTexture);
            info.SetPixelPortrait(pixelTexture);

            CardManager.Add("void", info);
        }
    }
}

