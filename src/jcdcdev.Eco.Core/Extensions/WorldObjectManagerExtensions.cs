using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;

namespace jcdcdev.Eco.Core.Extensions;

public static class WorldObjectManagerExtensions
{
    public static IEnumerable<StoreComponent> GetStores(this IWorldObjectManager manager) => manager.GetComponents<StoreComponent>();

    public static IEnumerable<T> GetComponents<T>(this IWorldObjectManager manager) where T : WorldObjectComponent =>
        manager.All
            .Where(y => y.HasComponent<T>())
            .Select(x => x.GetComponent<T>());
}