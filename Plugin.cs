using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using TemporalStormWeather.Compat;
using TemporalStormWeather.Compat.BCMERCompat;
using TemporalStormWeather.Debug;
using TemporalStormWeather.Patches;
using TemporalStormWeather.TemporalStormManagers;

namespace TemporalStormWeather;

[BepInDependency(BrutalCompanyMinusExtraRebornGUID, BepInDependency.DependencyFlags.SoftDependency)] // BCMER must be at least a soft dependency
[BepInPlugin(modGUID, modName, modVersion)]
internal class Plugin : BaseUnityPlugin
{
    private const string BrutalCompanyMinusExtraRebornGUID = "SoftDiamond.BrutalCompanyMinusExtraReborn";

    internal const string modGUID = "MrHat.TemporalStormWeather.Internals";
    internal const string modName = "TemporalStormWeather";
    internal const string modVersion = "0.0.1";

    internal static ManualLogSource? Mls { get; private set; }
    internal static Plugin? Instance { get; private set; }

    internal static bool hasBCMER;

    private readonly Harmony _harmony = new(modGUID);

    private void Awake()
    {
        Instance = this;
        Mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

        hasBCMER = Chainloader.PluginInfos.ContainsKey(BrutalCompanyMinusExtraRebornGUID);
		
        ConfigManager.BindConfigs();

        if (hasBCMER)
        {
            BCMERCompatibility.RegisterEvents(); // This should only be called in Plugin.Awake
            _harmony.PatchAll(typeof(BCMERCompatibility)); // For events `TemporalAllItems` & `RusticAllItems`
        }
    }
}
