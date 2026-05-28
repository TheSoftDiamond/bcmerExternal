#nullable enable

using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using TemporalStormWeather.API;
using TemporalStormWeather.Utils;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.WorldOfRust;

internal class WorldOfRust : MEvent
{
    public override string Name()
    {
        return nameof(WorldOfRust);
    }

    public override void Initalize()
    {
        Weight = 1;
        Descriptions =
        [
            "A Temporal Symphony"
        ];
        ColorHex = "#2C1507";
        Type = EventType.Insane;
        EventsToSpawnWith =
        [
            "RusticAllItems",
            "LocustSwarm"
        ];
        EventsToRemove =
        [
            "AllWeather",
            "BloodMoon",
            "Forsaken",
            "Gloomy",
            "Hallowed",
            "Heatwave",
            "HeavyRain",
            "Hurricane",
            "MajoraMoon",
            "MeteorShower",
            "Raining",
            "SolarFlare",
            "Windy"
        ];
        Aliases = ["WorldOfRust"];

        showTip = true;
        TipTitle = ["A Temporal Symphony"];
        TipMessages = ["This moon has collapsed to the World of Rust"];
        isWarning = true;

        ScaleList.Add(ScaleType.ScrapValue, new Scale(1.2f, 0.003f, 1.2f, 1.5f));
        ScaleList.Add(ScaleType.ScrapAmount, new Scale(1.15f, 0.003f, 1.15f, 1.45f));

        scrapTransmutationEvent = new ScrapTransmutationEvent(
            new Scale(0.08f, 0.0012f, 0.08f, 0.2f),
            new SpawnableItemWithRarity(Assets.GetItem("Temporal Gear"), 10),
            new SpawnableItemWithRarity(Assets.GetItem("Madness Gear"), 7)
        );
    }

    public override bool AddEventIfOnly()
    {
        return CustomWeather.isWeatherPresent("Temporal Storm");
    }

    public override void Execute()
    {
        BCMERCompatibility.SetExecuted(this, true);
        BCMERCompatibility.SetActive(this, true);
        CustomWeather.SetCustomWeather("Temporal Storm");
        TemporalInstabilityAPI.SetTemporalStormRoundActive(true);
        TemporalInstabilityAPI.SetServerInstabilityLevel(100f);
        Log.Debug("WorldOfRust event started");
    }

    public override void OnShipLeave()
    {
        StopEvent();
    }

    public override void OnGameStart()
    {
        StopEvent();
    }

    public override void OnLocalDisconnect()
    {
        StopEvent();
    }

    private void StopEvent()
    {
        BCMERCompatibility.SetActive(this, false);
        TemporalInstabilityAPI.SetTemporalStormRoundActive(false);
    }
}
