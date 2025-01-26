using System.Collections.Generic;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal interface IShopperSystem : ISystem
    {
        public bool IsShoppersAvailable { get; }

        public Vector3 RandomSpawnPoint { get; }

        public Vector3 KioskPoint { get; }

        public IEnumerable<ItemData> AvailableItems { get; }

        public bool TryGetShopper(out IShopperActor shopper);

        public IShopperActor SpawnRandomShopper(Vector3 position);

        public bool TrySpawnRandomShopper(Vector3 position, out IShopperActor shopper);

        public void AddShopper(IShopperActor shopper);

        public void RemoveShopper(IShopperActor shopper);

        public void RemoveDestination(IDestinationActor destination);

        public void AddDestination(IDestinationActor destination);

        public void RemoveAvailableShopper(IShopperActor shopper);
    }
}
