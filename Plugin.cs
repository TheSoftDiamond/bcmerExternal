#nullable enable

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

[BepInDependency(DawnLibGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(DuskGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(PathfindingLibGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(BasedDecorPlacementGUID, BepInDependency.DependencyFlags.SoftDependency)]
//[BepInDependency("com.github.zehsteam.ImmersiveEntrance", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(CullFactoryGUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(BrutalCompanyMinusExtraRebornGUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(modGUID, modName, modVersion)]
internal class Plugin : BaseUnityPlugin
{
    private const string DawnLibGUID = "com.github.teamxiaolan.dawnlib";
    private const string DuskGUID = "com.github.teamxiaolan.dawnlib.dusk";
    private const string PathfindingLibGUID = "Zaggy1024.PathfindingLib";
    private const string BasedDecorPlacementGUID = "MrHat.BasedDecorPlacement";
    //private const string ImmersiveEntranceGUID = "com.github.zehsteam.ImmersiveEntrance";
    private const string CullFactoryGUID = "com.fumiko.CullFactory";
    private const string BrutalCompanyMinusExtraRebornGUID = "SoftDiamond.BrutalCompanyMinusExtraReborn";

    internal const string modGUID = "MrHat.TemporalStormWeather.Internals";
    internal const string modName = "TemporalStormWeather";
    internal const string modVersion = "0.0.1";

    internal static ManualLogSource? Mls { get; private set; }
    internal static Plugin? Instance { get; private set; }

    internal static bool hasBasedDecorPlacement;
    //internal static bool hasImmersiveEntrance;
    internal static bool hasCullFactory;
    internal static bool hasBCMER;

    private readonly Harmony _harmony = new(modGUID);

    private void Awake()
    {
        Instance = this;
        Mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        hasBasedDecorPlacement = Chainloader.PluginInfos.ContainsKey(BasedDecorPlacementGUID);
        //hasImmersiveEntrance = Chainloader.PluginInfos.ContainsKey(ImmersiveEntranceGUID);
        hasCullFactory = Chainloader.PluginInfos.ContainsKey(CullFactoryGUID);
        hasBCMER = Chainloader.PluginInfos.ContainsKey(BrutalCompanyMinusExtraRebornGUID);
        ConfigManager.BindConfigs();

        Utils.Log.Info("                   +++     +++");
        Utils.Log.Info("            ++     ++++   ++++     ++");
        Utils.Log.Info("           +++++ +++++++++++++++ +++++");
        Utils.Log.Info("            +++++++++++++++++++++++++");
        Utils.Log.Info("      ++++++++++++++++++++++++++++++++++++++");
        Utils.Log.Info("      +++++++++++++++++++++++++++++++++++++");
        Utils.Log.Info("        +++++++++++    ++++    +++++++++++");
        Utils.Log.Info("  ++++++++++++++       ++++       ++++++++++++++");
        Utils.Log.Info(" +++++++++++++         ++++         +++++++++++++");
        Utils.Log.Info("     +++++++++++++   ++++++++   +++++++++++++");
        Utils.Log.Info("    ++++++++ ++++++++++++++++++++++++ ++++++++");
        Utils.Log.Info("+++++++++++     +++++++     ++++++     +++++++++++");
        Utils.Log.Info("+++++++++++      ++++        ++++      +++++++++++");
        Utils.Log.Info("    +++++++      ++++        ++++      +++++++");
        Utils.Log.Info("    +++++++     ++++++      ++++++     +++++++");
        Utils.Log.Info("++++++++++++ ++++++++++++++++++++++++ ++++++++++++");
        Utils.Log.Info(" ++++++++++++++++    ++++++++    ++++++++++++++++");
        Utils.Log.Info("     +++++++++         ++++         +++++++++");
        Utils.Log.Info("     +++++++++++       ++++        ++++++++++");
        Utils.Log.Info("   +++++++++++++++     ++++     +++++++++++++++");
        Utils.Log.Info("    +++  ++++++++++++++++++++++++++++++++  +++");
        Utils.Log.Info("          ++++++++++++++++++++++++++++++");
        Utils.Log.Info("         ++++++++++++++++++++++++++++++++");
        Utils.Log.Info("          ++    ++++++++++++++++++    ++");
        Utils.Log.Info("                ++++   ++++   ++++");
        Utils.Log.Info("                 ++    ++++    ++");

        _harmony.PatchAll(typeof(EnemyAIPatches));
        _harmony.PatchAll(typeof(EntranceTeleportPatches));
        _harmony.PatchAll(typeof(GameNetworkManagerPatches));
        _harmony.PatchAll(typeof(GrabbableObjectPatches));
        _harmony.PatchAll(typeof(HUDManagerPatches));
        _harmony.PatchAll(typeof(LungPropPatches));
        _harmony.PatchAll(typeof(NetworkManagerPatches));
        _harmony.PatchAll(typeof(PlayerControllerBPatches));
        _harmony.PatchAll(typeof(SoundManagerPatches));
        _harmony.PatchAll(typeof(StartOfRoundPatches));
        _harmony.PatchAll(typeof(ChatGlitchManager));
        _harmony.PatchAll(typeof(TimeOfDayPatches));

        if (hasBasedDecorPlacement)
        {
            BasedDecorPlacementCompat.RegisterSurfaceExtensions();
            Utils.Log.Info("BasedDecorPlacement compatibility enabled");
        }

        //if (HasImmersiveEntrance)
        //{
        //    _harmony.PatchAll(typeof(ImmersiveEntranceCompat));
        //    Utils.Log.Info("ImmersiveEntrance compatibility enabled");
        //}

        if (hasCullFactory)
        {
            _harmony.PatchAll(typeof(CullFactoryCompat));
            Utils.Log.Info("CullFactory compatibility enabled");
        }

        if (hasBCMER)
        {
            Utils.Log.Info("Brutal Company Minus Extra Reborn compatibility enabled");
            BCMERCompatibility.RegisterEvents();
            _harmony.PatchAll(typeof(BCMERCompatibility)); // For events `TemporalAllItems` & `RusticAllItems`
        }
    }
}
