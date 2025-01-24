using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal sealed class ShopperSystem : SimpleSystem, IShopperSystem
    {
        private readonly List<IShopperActor> shoppers = new();

        public bool TryGetShopper(out IShopperActor shopper)
        {
            shopper = shoppers.FirstOrDefault();
            return shopper != default;
        }

        public void AddShopper(IShopperActor shopper)
        {
            if (shoppers.Contains(shopper))
            {
                return;
            }

            shoppers.Add(shopper);
        }

        public void RemoveShopper(IShopperActor shopper)
        {
            shoppers.Remove(shopper);
        }
    }
}
