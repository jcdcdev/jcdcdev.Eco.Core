using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Shared.IoC;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core.Services;

public static class StoreService
{
    private static StoreLookup? _cache;

    public static StoreLookup Data => _cache ?? EnsureCache();

    public static void Update()
    {
        var seconds = TimeSpan.FromSeconds(Math.Min(CorePlugin.Config.StoreUpdateFrequency, 5));
        if (Data.Updated >= DateTime.UtcNow - seconds)
        {
            return;
        }

        EnsureCache();
    }

    private static StoreLookup EnsureCache()
    {
        var stores = GetAllStores();
        var data = MapStores(stores);
        var cache = StoreLookup.Create(data);
        _cache = cache;
        return cache;
    }

    private static Dictionary<Guid, Store> MapStores(IEnumerable<StoreComponent> stores)
    {
        var output = new Dictionary<Guid, Store>();
        foreach (var store in stores)
        {
            var data = Store.CreateFromStoreComponent(store);
            output.Add(data.Id, data);
        }

        return output;
    }

    private static IEnumerable<StoreComponent> GetAllStores() => ServiceHolder<IWorldObjectManager>.Obj.GetStores();
}