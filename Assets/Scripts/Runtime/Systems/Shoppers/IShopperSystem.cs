using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal interface IShopperSystem : ISystem
    {
        public bool TryGetShopper(out IShopperActor shopper);

        public void AddShopper(IShopperActor shopper);

        public void RemoveShopper(IShopperActor shopper);
    }
}
