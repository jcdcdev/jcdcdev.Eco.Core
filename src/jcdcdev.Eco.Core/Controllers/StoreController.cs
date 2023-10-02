using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Shared.IoC;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core.Controllers;

internal static class StoreController
{
    private static StoreCache? _cache;

    public static StoreCache Data => _cache ?? EnsureCache(); 

    public static void Update()
    {
        var seconds = TimeSpan.FromSeconds(Math.Min(CorePlugin.Config.StoreUpdateFrequency, 5));
        if (Data.Updated >= DateTime.UtcNow - seconds)
        {
            return;
        }
        EnsureCache();
    }

    private static StoreCache EnsureCache()
    {
        var stores = GetAllStores();
        var data = MapStores(stores);
        var cache = StoreCache.Create(data);
        _cache = cache;
        return cache;
    }

    private static Dictionary<Guid, Store> MapStores(List<StoreComponent> stores)
    {
        var output = new Dictionary<Guid, Store>();
        foreach (var store in stores)
        {
            var data = Store.CreateFromStoreComponent(store);
            output.Add(data.Id, data);
        }

        return output;
    }

    private static List<StoreComponent> GetAllStores()
    {
        var stores = ServiceHolder<IWorldObjectManager>.Obj.All.Where(y => y.HasComponent<StoreComponent>());
        return stores.Select(x => x.GetComponent<StoreComponent>()).ToList();
    }
}