using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Utilities;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal sealed class ShopperSystem : MonoSystem, IShopperSystem
    {
        [SerializeField]
        private GameplaySettings gameplaySettings;

        private readonly List<IDestinationActor> destinations = new();
        private readonly List<IShopperActor> spawnedShoppers = new();

        public Vector3 RandomSpawnPoint
        {
            get
            {
                var spawnPoints = destinations.OfType<ShopperSpawnPointActor>().ToList();
                var spawnPoint = spawnPoints.GetRandom();

                return spawnPoint.Position;
            }
        }

        public Vector3 KioskPoint
        {
            get
            {
                var kioskDestination = destinations.Find(destination => destination is KioskPointActor);
                return kioskDestination.Position;
            }
        }

        public bool TryGetShopper(out IShopperActor shopper)
        {
            shopper = spawnedShoppers.FirstOrDefault();
            return shopper != default;
        }

        public IShopperActor SpawnRandomShopper(Vector3 position)
        {
            var randomShopperData = gameplaySettings.AvailableShoppers.GetRandom();
            var shopperPrefab = randomShopperData.ShopperPrefab;
            var shopper = Instantiate(shopperPrefab, position, Quaternion.identity);
            shopper.Initialize(randomShopperData);

            return shopper;
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

        public void AddDestination(IDestinationActor destination)
        {
            if (destinations.Contains(destination))
            {
                return;
            }

            destinations.Add(destination);
        }

        public void RemoveDestination(IDestinationActor destination)
        {
            destinations.Remove(destination);
        }
    }
}
