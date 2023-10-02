using Eco.Gameplay.Components;
using jcdcdev.Eco.Core.Extensions;

namespace jcdcdev.Eco.Core.Models;

public class Store
{
    private Store(Guid id, List<TradeOffer> selling, List<TradeOffer> buying)
    {
        Id = id;
        Selling = selling;
        Buying = buying;
        AvgCostPerThousandCalories = selling.CalculateAvgCaloriesCost();
        AvgCostPerThousandCaloriesInStock = selling.CalculateAvgCaloriesCost(false);
    }

    public Guid Id { get; }
    public List<TradeOffer> Selling { get; }
    public List<TradeOffer> Buying { get; }
    public float? AvgCostPerThousandCalories { get; }
    public float? AvgCostPerThousandCaloriesInStock { get; }

    public static Store CreateFromStoreComponent(StoreComponent store)
    {
        var id = store.Parent.ObjectID;
        var selling = store.AllOffers.Where(x => !x.Buying).ToList();
        var buying = store.AllOffers.Where(x => x.Buying).ToList();

        var data = new Store(id, selling, buying);
        return data;
    }
}