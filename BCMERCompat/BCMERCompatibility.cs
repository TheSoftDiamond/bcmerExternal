#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BrutalCompanyMinus.Minus;
using HarmonyLib;
using TemporalStormWeather.Compat.BCMERCompat.Events.LocustSwarm;
using TemporalStormWeather.Compat.BCMERCompat.Events.RusticAllItems;
using TemporalStormWeather.Compat.BCMERCompat.Events.TemporalAllItems;
using TemporalStormWeather.Compat.BCMERCompat.Events.WorldOfRust;
using TemporalStormWeather.Utils;

namespace TemporalStormWeather.Compat.BCMERCompat;

internal static class BCMERCompatibility
{
    private static readonly FieldInfo Active = typeof(MEvent).GetField("Active", BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static readonly FieldInfo Executed = typeof(MEvent).GetField("Executed", BindingFlags.Instance | BindingFlags.NonPublic)!;

    internal static void RegisterEvents()
    {
        RegisterEvent(EventManager.moddedEvents, new RusticAllItems());
        RegisterEvent(EventManager.moddedEvents, new LocustSwarm());
        RegisterEvent(EventManager.moddedEvents, new WorldOfRust());
        RegisterEvent(EventManager.moddedEvents, new TemporalAllItems());
    }

    private static void RegisterEvent(List<MEvent> moddedEvents, MEvent moddedEvent)
    {
        string eventName = moddedEvent.Name();

        if (moddedEvents.Any(registeredEvent => registeredEvent.Name() == eventName))
        {
            return;
        }

        moddedEvents.Add(moddedEvent);
        Log.Info($"BCMER {eventName} modded event registered");
    }

    internal static void SetActive(MEvent moddedEvent, bool active)
    {
        Active.SetValue(moddedEvent, active);
    }

    internal static void SetExecuted(MEvent moddedEvent, bool executed)
    {
        Executed.SetValue(moddedEvent, executed);
    }

    [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start))]
    [HarmonyPostfix]
    private static void Postfix(GrabbableObject __instance)
    {
        _ = RusticAllItemsBehaviour.TryAttachItem(__instance);
        _ = TemporalAllItemsBehaviour.TryApplyItem(__instance);
    }
}
