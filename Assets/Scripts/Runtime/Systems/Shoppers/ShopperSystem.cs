using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal sealed class ShopperSystem : SimpleSystem, IShopperSystem
    {
        private readonly List<IShopperActor> spawnedShoppers = new();

        public bool TryGetShopper(out IShopperActor shopper)
        {
            shopper = spawnedShoppers.FirstOrDefault();
            return shopper != default;
        }

        public IShopperActor SpawnRandomShopper()
        {
            return default;
        }

        public void AddShopper(IShopperActor shopper)
        {
            if (spawnedShoppers.Contains(shopper))
            {
                return;
            }

            spawnedShoppers.Add(shopper);
        }

        public void RemoveShopper(IShopperActor shopper)
        {
            spawnedShoppers.Remove(shopper);
        }
    }
}
