/// TODO NOW: check if cases where maxDeathDelay(or similar)>5 on postmortem
/// TODO: document behaviours when PartyMaxDeathDelay.Value < 1 and make fix
/// TODO: check for unused references from UltimateSuite
/// TODO: allow party usage if all clients installed
/// TODO COMPATIBILITY: redo camera follow patch
/// TODO COMPATIBILITY: deny challenge usage if using UltimateSuite

using HarmonyLib;
using BepInEx.Configuration;

namespace BetterRespawnMod.Patches
{
    [HarmonyPatch(typeof(Character))]
    internal class MaxDeathDelayPatch
    {
        private static ConfigEntry<float> FreeplayMaxDeathDelay;
        private static ConfigEntry<float> ChallengeMaxDeathDelay;
        //private static ConfigEntry<float> PartyMaxDeathDelay;
        //private static ConfigEntry<float> CreativeMaxDeathDelay;

        public static void InitializeConfig(ConfigFile config)
        {
            FreeplayMaxDeathDelay = config.Bind(
                "MaxDeathDelay",
                "FreeplayMode",
                0.25f
            );

            ChallengeMaxDeathDelay = config.Bind(
                "MaxDeathDelay",
                "ChallengeMode",
                5f,
                new ConfigDescription(
                    "Keep default if completion requires postmortem!\n" +
                    "Use 0 if also using UltimateSuite.",           // SetupForChallengeModePatch.cs
                    new AcceptableValueRange<float>(0.0f, 5.0f)     // denies extended postmortem window advantage
                )
            );

            //PartyMaxDeathDelay = config.Bind(
            //    "MaxDeathDelay",
            //    "PartyMode",
            //    5f,
            //    "Lower than 1 leads to bugs!"
            //);

            //CreativeMaxDeathDelay = config.Bind(
            //    "MaxDeathDelay",
            //    "CreativeMode",
            //    5f,
            //);
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void PatchMaxDeathDelay(ref float ___maxDeathDelay)
        {
            var gameMode = GameSettings.GetInstance().GameMode;
            switch (gameMode)
            {
                case GameState.GameMode.FREEPLAY:
                    ___maxDeathDelay = FreeplayMaxDeathDelay.Value;
                    break;
                case GameState.GameMode.CHALLENGE:
                    ___maxDeathDelay = ChallengeMaxDeathDelay.Value;
                    break;
                //case GameState.GameMode.PARTY:
                //    ___maxDeathDelay = PartyMaxDeathDelay.Value;
                //    break;
                //case GameState.GameMode.CREATIVE:
                //    ___maxDeathDelay = CreativeMaxDeathDelay.Value;
                //    break;
            }
        }
    }
}