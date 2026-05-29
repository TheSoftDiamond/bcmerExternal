using BrutalCompanyMinus.Minus;
using TemporalStormWeather.Items;
using TemporalStormWeather.Utils;
using UnityEngine;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.TemporalAllItems;

internal class TemporalAllItemsBehaviour : MonoBehaviour
{
    private static TemporalAllItemsBehaviour? current;

    private MEvent? activeEvent;

    internal static bool IsActive => current?.activeEvent != null && BrutalCompanyMinus.Minus.API.IsEventActive(current.activeEvent);

    internal static void StartFor(MEvent customEvent)
    {
        if (current == null)
        {
            GameObject Object = new(Plugin.modName + ".TemporalAllItems");
            current = Object.AddComponent<TemporalAllItemsBehaviour>();
        }

        current.Begin(customEvent);
    }

    internal static void StopFor(MEvent customEvent)
    {
        if (current == null || current.activeEvent != customEvent)
        {
            return;
        }

        Destroy(current.gameObject);
    }

    private void Begin(MEvent customEvent)
    {
        activeEvent = customEvent;
        ApplyExistingItems();
    }

    internal static bool TryApplyItem(GrabbableObject item)
    {
        return IsActive
            && NetworkUtils.IsServerOrNotNetworked()
            && !item.isInShipRoom
            && ItemEffect.TryAttach(item, out ItemEffect? itemEffect, true)
            && itemEffect?.ForceTemporal() == true;
    }

    private static void ApplyExistingItems()
    {
        if (!NetworkUtils.IsServerOrNotNetworked())
        {
            return;
        }

        GrabbableObject[] items = FindObjectsByType<GrabbableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        for (int i = 0; i < items.Length; i++)
        {
            TryApplyItem(items[i]);
        }
    }

    private void OnDestroy()
    {
        if (current == this)
        {
            current = null;
        }
    }
}
