using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperSpawnState : GameplayState
    {
        private readonly GameplaySettings gameplaySettings;
        private IShopperSystem shopperSystem;

        private float spawnTimeSeconds;

        public ShopperSpawnState(GameplaySettings gameplaySettings)
        {
            this.gameplaySettings = gameplaySettings;
        }

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
        }

        protected override void OnDisposed()
        {
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            spawnTimeSeconds = Time.time + gameplaySettings.SpawnDelaySeconds;
        }

        protected override void OnExited(GameplayStateContext context)
        {
            spawnTimeSeconds = 0f;
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            if (Time.time < spawnTimeSeconds)
            {
                return Status.Working;
            }

            if (shopperSystem.TrySpawnRandomShopper(shopperSystem.RandomSpawnPoint, out var shopper))
            {
                context.ActiveShopper = shopper;
                return Status.Completed;
            }

            return Status.Working;
        }
    }
}
