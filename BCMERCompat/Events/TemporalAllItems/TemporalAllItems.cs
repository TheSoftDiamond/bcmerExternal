using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus;
using TemporalStormWeather.Utils;
using static BrutalCompanyMinus.Minus.Manager;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.TemporalAllItems;

internal class TemporalAllItems : MEvent
{
    public override string Name()
    {
        return nameof(TemporalAllItems);
    }

    public override void Initalize()
    {
        Weight = 2;
        Descriptions =
        [
            "Temporal energy flows through every item"
        ];
        ColorHex = "#19B476";
        Type = EventType.Rare;
        Aliases = ["TemporalAllItems"];
        EventsToRemove =
        [
            "WorldOfRust",
            "RusticAllItems"
        ];
    }

    public override void Execute()
    {
        BCMERCompatibility.SetExecuted(this, true);
        BCMERCompatibility.SetActive(this, true);
        TemporalAllItemsBehaviour.StartFor(this);
        Log.Debug("TemporalAllItems event started");
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
        TemporalAllItemsBehaviour.StopFor(this);
    }
}
