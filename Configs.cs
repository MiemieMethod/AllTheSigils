using APIPlugin;
using BepInEx.Configuration;
using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;



namespace AllTheSigils
{
    public partial class Plugin
    {
        internal static ConfigEntry<bool> configAcidTrail;
        internal static ConfigEntry<bool> configAgile;
        internal static ConfigEntry<bool> configAppetizing;
        internal static ConfigEntry<bool> configBloodGuzzler;
        internal static ConfigEntry<bool> configBombardier;
        internal static ConfigEntry<bool> configBurning;
        internal static ConfigEntry<bool> configConsumer;
        internal static ConfigEntry<bool> configCowardly;
        internal static ConfigEntry<bool> configDeathburst;
        internal static ConfigEntry<bool> configMultiStrike;
        internal static ConfigEntry<bool> configDying;
        internal static ConfigEntry<bool> configRecoil;
        internal static ConfigEntry<bool> configParalysis;
        internal static ConfigEntry<bool> configElectric;
        internal static ConfigEntry<bool> configFamiliar;
        internal static ConfigEntry<bool> configLeech;
        internal static ConfigEntry<bool> configPossessor;
        internal static ConfigEntry<bool> configPrideful;
        internal static ConfigEntry<bool> configRegen;
        internal static ConfigEntry<bool> configResistant;
        internal static ConfigEntry<bool> configSickness;
        internal static ConfigEntry<bool> configThickShell;
        internal static ConfigEntry<bool> configThief;
        internal static ConfigEntry<bool> configToxin;
        internal static ConfigEntry<bool> configTrample;
        internal static ConfigEntry<bool> configVicious;


        internal static ConfigEntry<bool> configHammerBlock;

        public void AddConfigs()
        {
            configAcidTrail = Config.Bind("Good Sigil", "Acid Trail", true, "Should Leshy have this?");
            configAgile = Config.Bind("Good Sigil", "Agile", true, "Should Leshy have this?");
            configBloodGuzzler = Config.Bind("Good Sigil", "BloodGuzzler", true, "Should Leshy have this?");
            configConsumer = Config.Bind("Good Sigil", "Consumer", true, "Should Leshy have this?");
            configDeathburst = Config.Bind("Good Sigil", "Deathburst", true, "Should Leshy have this?");
            configElectric = Config.Bind("Good Sigil", "Electric", true, "Should Leshy have this?");
            configMultiStrike = Config.Bind("Good Sigil", "MultiStrike", true, "Should Leshy have this?");
            configFamiliar = Config.Bind("Good Sigil", "Familiar", true, "Should Leshy have this?");
            configLeech = Config.Bind("Good Sigil", "Leech", true, "Should Leshy have this?");
            configPossessor = Config.Bind("Good Sigil", "Possessor", true, "Should Leshy have this?");
            configRegen = Config.Bind("Good Sigil", "Regen 1", true, "Should Leshy have this?");
            configResistant = Config.Bind("Good Sigil", "Resistant", true, "Should Leshy have this?");
            configThickShell = Config.Bind("Good Sigil", "Thick Shell", true, "Should Leshy have this?");
            configThief = Config.Bind("Good Sigil", "Thief", true, "Should Leshy have this?");
            configToxin = Config.Bind("Good Sigil", "Toxins (all of them)", true, "Should Leshy have this?");
            configTrample = Config.Bind("Good Sigil", "Trample", true, "Should Leshy have this?");
            configVicious = Config.Bind("Good Sigil", "Vicious", true, "Should Leshy have this?");

            configAppetizing = Config.Bind("Bad Sigil", "Appetizing", true, "Should Leshy have this?");
            configBurning = Config.Bind("Bad Sigil", "Burning", true, "Should Leshy have this?");
            configCowardly = Config.Bind("Bad Sigil", "Cowardly", true, "Should Leshy have this?");
            configDying = Config.Bind("Bad Sigil", "Dying", true, "Should Leshy have this?");
            configParalysis = Config.Bind("Bad Sigil", "Paralysis", true, "Should Leshy have this?");
            configRecoil = Config.Bind("Bad Sigil", "Recoil", true, "Should Leshy have this?");
            configPrideful = Config.Bind("Bad Sigil", "Prideful", true, "Should Leshy have this?");
            configSickness = Config.Bind("Bad Sigil", "Sickness", true, "Should Leshy have this?");

            configBombardier = Config.Bind("Chaos Sigil", "Bombardier", true, "Should Leshy have this?");

            configHammerBlock = Config.Bind("Hammer Block", "Pathetic Sacrifice", true, "Should the sigil pathetic sacrifice be invalid for hammering? Due to the intent being it is stuck on your board. default is true.");
        }
    }
}