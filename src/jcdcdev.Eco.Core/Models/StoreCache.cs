namespace jcdcdev.Eco.Core.Models;

internal class StoreCache
{
    public static StoreCache Create(Dictionary<Guid, Store> stores) => new(stores, DateTime.Now);

    private StoreCache(Dictionary<Guid, Store> stores, DateTime updated)
    {
        Stores = stores;
        Updated = updated;
    }

    public DateTime Updated { get; }
    public Dictionary<Guid, Store> Stores { get; }
    public float? AvgCostPerThousandCalories => Stores.Sum(x => x.Value.AvgCostPerThousandCalories);
    public float? AvgCostPerThousandCaloriesInStock => Stores.Sum(x => x.Value.AvgCostPerThousandCalories);
    public Store? Get(Guid id) => Stores.TryGetValue(id, out var store) ? store : null;
}