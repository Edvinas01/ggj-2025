using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperSpawnState : GameplayState
    {
        private IShopperSystem shopperSystem;

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            var shopper = shopperSystem.SpawnRandomShopper(shopperSystem.RandomSpawnPoint);
            context.ActiveShopper = shopper;
        }

        protected override void OnExited(GameplayStateContext context)
        {
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            return Status.Completed;
        }
    }
}
