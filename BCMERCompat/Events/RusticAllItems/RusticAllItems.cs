using System;
using BrutalCompanyMinus.Minus;
using BrutalCompanyMinus.Minus.Handlers.Modded;
using TemporalStormWeather.API;
using TemporalStormWeather.Utils;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.RusticAllItems;

internal class RusticAllItems : MEvent
{
    public override string Name()
    {
        return nameof(RusticAllItems);
    }

    public override void Initalize()
    {
        Weight = 2;
        Descriptions =
        [
            "Rust spreads through every item"
        ];
        ColorHex = "#B87333";
        Type = EventType.Bad;
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
        Aliases = ["RusticAllItems"];
    }

    public override bool AddEventIfOnly()
    {
        return string.Equals(CustomWeather.GetCustomWeather(), "Temporal Storm");
    }

    public override void Execute()
    {
        BCMERCompatibility.SetExecuted(this, true);
        BCMERCompatibility.SetActive(this, true);
        TemporalInstabilityAPI.SetTemporalStormRoundActive(true);
        RusticAllItemsBehaviour.StartFor(this);
        Log.Debug("RusticAllItems event started");
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
        RusticAllItemsBehaviour.StopFor(this);
        TemporalInstabilityAPI.SetTemporalStormRoundActive(false);
    }
}
