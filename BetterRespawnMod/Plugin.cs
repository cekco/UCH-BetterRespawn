using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BetterRespawnMod.Patches;

namespace BetterRespawnMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "BetterRespawnMod";
        private const string modName = "BetterRespawnMod";
        private const string modVersion = "0.0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static Plugin Instance;

        internal ManualLogSource mls;

        void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("beepboop beepboop");

            MaxDeathDelayPatch.InitializeConfig(Config);

            if (Instance == null)
            {
                Instance = this;
            }

            harmony.PatchAll();
        }
    }
}