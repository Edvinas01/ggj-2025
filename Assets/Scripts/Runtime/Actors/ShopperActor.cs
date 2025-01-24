using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ShopperActor : MonoBehaviour, IShopperActor
    {
        private IShopperSystem shopperSystem;

        public string Name => name;

        private void Awake()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
        }

        private void OnEnable()
        {
            shopperSystem.AddShopper(this);
        }

        private void OnDisable()
        {
            shopperSystem.RemoveShopper(this);
        }

        public void WalkTo(Vector3 position)
        {
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
