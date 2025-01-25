using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.Utilities;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal sealed class ShopperSystem : MonoSystem, IShopperSystem
    {
        [SerializeField]
        private GameplaySettings gameplaySettings;

        private List<ShopperData> availableShoppers = new();
        private readonly List<IDestinationActor> destinations = new();
        private readonly List<IShopperActor> spawnedShoppers = new();

        public bool IsShoppersAvailable => availableShoppers.Count > 0;

        public Vector3 RandomSpawnPoint
        {
            get
            {
                if (destinations.Count <= 0)
                {
                    return Vector3.zero;
                }

                var spawnPoints = destinations.OfType<ShopperSpawnPointActor>().ToList();
                if (spawnPoints.Count <= 0)
                {
                    return Vector3.zero;
                }

                var spawnPoint = spawnPoints.GetRandom();

                return spawnPoint.Position;
            }
        }

        public Vector3 KioskPoint
        {
            get
            {
                var kioskDestination = destinations.FirstOrDefault(destination => destination is KioskPointActor);
                if (kioskDestination == default)
                {
                    return Vector3.zero;
                }

                return kioskDestination.Position;
            }
        }

        public override void OnInitialized()
        {
            GameManager.AddListener<SceneLoadEnteredMessage>(OnSceneLoadEntered);
        }

        public override void OnDisposed()
        {
            GameManager.RemoveListener<SceneLoadEnteredMessage>(OnSceneLoadEntered);
        }

        public bool TryGetShopper(out IShopperActor shopper)
        {
            shopper = spawnedShoppers.FirstOrDefault();
            return shopper != default;
        }

        public bool TrySpawnRandomShopper(Vector3 position, out IShopperActor shopper)
        {
            if (availableShoppers.Count <= 0)
            {
                shopper = default;
                return false;
            }

            var randomShopperData = availableShoppers.GetRandom();
            var shopperPrefab = randomShopperData.ShopperPrefab;

            var shopperActor = Instantiate(shopperPrefab, position, Quaternion.identity);
            shopperActor.Initialize(randomShopperData);

            shopper = shopperActor;

            return true;
        }

        public IShopperActor SpawnRandomShopper(Vector3 position)
        {
            var randomShopperData = availableShoppers.GetRandom();
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

        public void RemoveAvailableShopper(IShopperActor shopper)
        {
            availableShoppers.Remove(shopper.Data);
        }

        public void RemoveDestination(IDestinationActor destination)
        {
            destinations.Remove(destination);
        }

        private void OnSceneLoadEntered(SceneLoadEnteredMessage message)
        {
            availableShoppers.Clear();
            availableShoppers = gameplaySettings.AvailableShoppers.Select(data => data.Copy()).ToList();
        }
    }
}
