using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UABPetelnia.GGJ2025.Runtime.Utilities;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ShopperSpawnPointActor : MonoBehaviour, IDestinationActor
    {
        private IShopperSystem shopperSystem;

        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            GizmoLocations.DrawLocationMarker(position: Position, label: name, lineHeight: 5f, color: Color.green);
        }

        private void Awake()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
        }

        private void OnEnable()
        {
            shopperSystem.AddDestination(this);
        }

        private void OnDisable()
        {
            shopperSystem.RemoveDestination(this);
        }
    }
}
