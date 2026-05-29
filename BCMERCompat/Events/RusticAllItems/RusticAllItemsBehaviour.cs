using BrutalCompanyMinus.Minus;
using TemporalStormWeather.Items;
using TemporalStormWeather.Utils;
using UnityEngine;

namespace TemporalStormWeather.Compat.BCMERCompat.Events.RusticAllItems;

internal class RusticAllItemsBehaviour : MonoBehaviour
{
    private static RusticAllItemsBehaviour? current;

    private MEvent? activeEvent;

    internal static bool IsActive => current?.activeEvent != null && BrutalCompanyMinus.Minus.API.IsEventActive(current.activeEvent);

    internal static void StartFor(MEvent customEvent)
    {
        if (current == null)
        {
            GameObject Object = new(Plugin.modName + ".RusticAllItems");
            current = Object.AddComponent<RusticAllItemsBehaviour>();
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
        AttachExistingItems();
    }

    internal static bool TryAttachItem(GrabbableObject item)
    {
        return IsActive
               && NetworkUtils.IsServerOrNotNetworked()
               && !item.isInShipRoom
               && ItemEffect.TryAttach(item, out _, true);
    }

    private static void AttachExistingItems()
    {
        if (!NetworkUtils.IsServerOrNotNetworked())
        {
            return;
        }

        GrabbableObject[] items = FindObjectsByType<GrabbableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        for (int i = 0; i < items.Length; i++)
        {
            TryAttachItem(items[i]);
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
