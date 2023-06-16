using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AllTheSigils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public partial class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "ATS";

        public const string OldLilyPluginGuid = "org.memez4life.inscryption.customsigils";
        public const string OldVoidPluginGuid = "extraVoid.inscryption.voidSigils";
        public const string OldAnthonyPluginGuid = "AnthonyPython.inscryption.AnthonysSigils";

        public static List<String> OldPluginGuids = new List<String>() { OldLilyPluginGuid, OldVoidPluginGuid, OldAnthonyPluginGuid };

        private const string PluginName = "AllTheSigils";

        private const string PluginVersion = "1.0.0";

        public static string Directory;

        internal static ManualLogSource Log;

        public static bool voidCombatPhase;

        public static Dictionary<Ability, String> NewSigilVersions = new Dictionary<Ability, string>();

        private void Awake()
        {
            base.Logger.LogInfo("Loaded AllTheSigils!");
            Plugin.Log = base.Logger;

            voidCombatPhase = false;
            AddConfigs();

            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();

            //Old Lily's sigils
            AddBond();
            AddShort();
            AddTribe_Attack();
            AddTribe_Health();
            AddBi_Blood();
            AddQuadra_Blood();
            AddImbuing();
            AddHydra();
            AddFight_Activated();
            AddPicky();
            AddFearful();
            AddTwoDeathBones();
            AddThreeDeathBones();
            AddFiveDeathBones();
            AddSixDeathBones();
            AddOneSummonBones();
            AddTwoSummonBones();
            AddThreeSummonBones();
            AddHost();
            AddSpawner();
            AddLauncher();
            AddShape_Shifter();
            AddDraw_Side_Deck_OnSummon();
            AddBait();
            AddLinguist();
            AddAll_Seeing();
            AddBlood_Shifter();
            AddAsleep();
            AddSong_Of_Sleep();
            AddWarper();
            AddRushing_March();
            AddWild_Hunger();
            AddRight_Slash();
            AddLeft_Slash();
            AddDouble_Slash();
            AddPuppets_Gift();
            AddInstakill();


            //Add Card
            Voids_work.Cards.Acid_Puddle.AddCard();
            Voids_work.Cards.Jackalope.AddCard();

            //Attack sigils
            AddAcidTrail();
            AddAmbush();
            AddDeathburst();
            AddEletric();
            AddEnforcer();
            AddInsectKiller();
            AddFamiliar();
            AddDoubleAttack();
            AddHaste();
            AddRetaliate();
            AddStampede();
            AddSubmergedAmbush();
            AddPierce();
            AddTrample();

            //Buff Attack Sigils
            AddDesperation();
            AddZapper();
            AddGiant();
            AddLifeStatsUp();
            AddPredator();
            AddOpportunist();
            AddVicious();

            //Defensive sigils
            AddAgile();
            AddBodyguard();
            AddCaustic();
            AddGrazing();
            AddMedic();
            AddRegenFull();
            AddRegen1();
            AddRegen2();
            AddRegen3();
            AddResistant();
            AddProtector();
            AddPoisonous();
            AddThickShell();

            //Debuff sigils
            AddDwarf();
            AddFireStarter();
            AddShocker();
            AddToxin();
            AddToxinStrength();
            AddToxinVigor();
            AddToxinDeadly();
            AddToxinSickly();

            //Negative Sigils
            AddAppetizing();
            AddBlight();
            AddBlind();
            AddBroken();
            AddBombardier();
            AddBoneless();
            AddBurning();
            AddCoward();
            AddDying();
            AddPathetic();
            AddParalise();
            AddPrideful();
            AddRecoil();
            AddSickness();
            AddToothpicker();
            AddTransient();
            AddWithering();

            //Utility Sigils
            AddAntler();
            AddAbundance();
            AddBloodGrowth();
            AddBloodGuzzler();
            AddBonePicker();
            AddBox();
            AddConsumer();
            addCoinFinder();
            AddTakeAllNegatives();
            AddDrawBlood();
            AddDrawBone();
            AddDrawIce();
            AddDrawJack();
            AddFishHook();
            AddFrightful();
            AddLeech();
            AddFisher();
            AddHerd();
            AddHourglass();
            AddManeuver();
            AddMidas();
            AddNutritious();
            AddPossessor();
            AddRandomStrafe();
            AddShove();
            AddRepellant();
            AddScissors();
            AddThief();
            AddTooth();
            AddTribalAlly();
            AddTribalTutor();
            AddStrafePowerUp();
            AddMovingPowerUp();

            foreach (AbilityInfo ability in AbilityManager.AllAbilityInfos)
            {
                string guid = GetSigilGuid(ability.ability);

                if (OldPluginGuids.Contains(guid))
                {
                    AbilityInfo info = AbilityManager.New(
                        PluginGuid,
                        ability.rulebookName,
                        ability.rulebookDescription,
                        typeof(EmptySigil),
                        GetTexture("placeholder")
                    );
                    NewSigilVersions.Add(info.ability, guid);
                }
            }

            //AddDevStuff();
        }

        public void Start()
        {
            ReplaceNewSigilsOnCards();
        }

        public void ReplaceNewSigilsOnCards()
        {

            for (int i = 0; i < CardManager.BaseGameCards.Count; i++)
            {
                CardInfo card = CardManager.BaseGameCards[i];
                for (int j = 0; j < card.Abilities.Count; j++)
                {
                    Ability ability = card.Abilities[j];

                    if (NewSigilVersions.ContainsKey(ability))
                    {
                        CardManager.BaseGameCards[i].abilities[j] = GetCustomAbility(NewSigilVersions[ability], AbilitiesUtil.GetInfo(ability).rulebookName);
                    }
                }
            }

            NewSigilVersions.Keys.ToList().ForEach(x => AbilityManager.Remove(x));
        }

        public string GetSigilGuid(Ability ability)
        {
            Dictionary<string, Dictionary<string, object>> SaveData = (Dictionary<string, Dictionary<string, object>>)AccessTools.Field(typeof(ModdedSaveData), "SaveData").GetValue(ModdedSaveManager.SaveData);

            foreach (KeyValuePair<string, object> keyValuePair in SaveData["cyantist.inscryption.api"])
            {
                List<string> SubstringList = keyValuePair.Key.Split('_').ToList();
                if (SubstringList[0] == "Ability")
                {
                    int AbilityID;
                    if (!int.TryParse(ability.ToString(), out AbilityID))
                    {
                        continue;
                    }
                    int ValueID = int.Parse((string)keyValuePair.Value);

                    if (ValueID == AbilityID)
                    {
                        return SubstringList[1];
                    }
                }
            }
            return null;
        }

        private void AddDevStuff()
        {
            AddDev_Activated();
            CardInfo Squirrel = CardManager.BaseGameCards.CardByName("Squirrel");
            Squirrel.abilities = new List<Ability> { Wild_Hunger.ability };

            CardInfo Geck = CardManager.BaseGameCards.CardByName("Geck");
            Geck.abilities = new List<Ability> { Ability.IceCube };
            Geck.SetIceCube(CardLoader.GetCardByName("Amalgam"));
        }

        public Texture2D GetTexture(string path, bool Act2 = false)
        {
            string folder = Act2 ? "Act2" : "Act1";
            byte[] imgBytes = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(base.Info.Location), "Artwork/", "Lily", $"{folder}/", $"{path}.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        public static Ability GetCustomAbility(string GUID, string rulebookname)
        {
            return GuidManager.GetEnumValue<Ability>(GUID, rulebookname);
        }
    }
}
