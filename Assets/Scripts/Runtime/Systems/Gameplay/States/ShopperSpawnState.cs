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

        protected override void OnDisposed()
        {
        }

        protected override void OnEntered(GameplayStateContext context)
        {
        }

        protected override void OnExited(GameplayStateContext context)
        {
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            if (shopperSystem.TrySpawnRandomShopper(shopperSystem.RandomSpawnPoint, out var shopper))
            {
                context.ActiveShopper = shopper;
                return Status.Completed;
            }

            return Status.Working;
        }
    }
}
