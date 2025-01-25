using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Shoppers
{
    internal interface IShopperSystem : ISystem
    {
        public Vector3 RandomSpawnPoint { get; }

        public Vector3 KioskPoint { get; }

        public bool TryGetShopper(out IShopperActor shopper);

        public IShopperActor SpawnRandomShopper(Vector3 position);

        public void AddShopper(IShopperActor shopper);

        public void RemoveShopper(IShopperActor shopper);

        public void RemoveDestination(IDestinationActor destination);

        public void AddDestination(IDestinationActor destination);
    }
}
