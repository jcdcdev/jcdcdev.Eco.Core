using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Shared.IoC;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Models;

namespace jcdcdev.Eco.Core.Services;

public static class StoreService
{
    public static StoreLookup Data { get; private set; } = StoreLookup.Empty();

    public static void Update(uint cutoff)
    {
        var seconds = TimeSpan.FromSeconds(Math.Max(cutoff, 5));
        if (Data.Updated >= DateTime.UtcNow - seconds)
        {
            return;
        }

        var stores = ServiceHolder<IWorldObjectManager>.Obj.GetStores();
        var data = MapStores(stores);
        Data = StoreLookup.Create(data);
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
}