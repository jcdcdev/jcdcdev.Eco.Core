using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Shared.Utils;

namespace jcdcdev.Eco.Core.Extensions;

public static class TradeOfferExtensions
{
    public static float CalculateAvgCaloriesCost(this IEnumerable<TradeOffer> offers, bool includeOutOfStock = true)
    {
        var costs = new Dictionary<Item, float>();
        var grouped = offers
            .GroupBy(x => x.Stack.Item)
            .OrderBy(x => x.Key?.Name);

        foreach (var offer in grouped)
        {
            var item = offer.Key;
            if (item is not FoodItem foodItem)
            {
                continue;
            }

            if (foodItem.Calories <= 0)
            {
                continue;
            }

            if (offer.Sum(x => x.Stack.Quantity) == 0)
            {
                if (!includeOutOfStock)
                {
                    continue;
                }
            }

            if (offer.Sum(x => x.Price) == 0)
            {
                costs.Add(item, 0);
                continue;
            }

            var totalQuantity = offer.Sum(x => x.Stack.Quantity);
            var totalPrice = offer.Sum(x => x.Price * x.Stack.Quantity);
            var avgPrice = totalPrice / totalQuantity;
            var costPerCalorie = avgPrice / foodItem.Calories;
            var costPerThousand = costPerCalorie * 1000;

            Log.Debug(
                $"{foodItem.Name} Quantity: {totalQuantity} Cost: {totalPrice} Avg Price: {avgPrice} Cost Per 1000 Cal: {costPerThousand}");
            costs.Add(item, costPerThousand);
        }

        if (!costs.Any())
        {
            return 0;
        }

        if (costs.Sum(x => x.Value) == 0)
        {
            return 0;
        }

        return costs.Average(x => x.Value);
    }
}