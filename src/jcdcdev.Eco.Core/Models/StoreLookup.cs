namespace jcdcdev.Eco.Core.Models;

public class StoreLookup
{
    private StoreLookup(Dictionary<Guid, Store> stores, DateTime updated)
    {
        Stores = stores;
        Updated = updated;
    }

    public DateTime Updated { get; }
    public Dictionary<Guid, Store> Stores { get; }
    public float? AvgCostPerThousandCalories => Stores.Sum(x => x.Value.AvgCostPerThousandCalories);
    public float? AvgCostPerThousandCaloriesInStock => Stores.Sum(x => x.Value.AvgCostPerThousandCaloriesInStock);
    public static StoreLookup Create(Dictionary<Guid, Store> stores) => new(stores, DateTime.UtcNow);

    public static StoreLookup Empty() => new(new Dictionary<Guid, Store>(), DateTime.MinValue);

    public Store? Get(Guid id) => Stores.TryGetValue(id, out var store) ? store : null;
}