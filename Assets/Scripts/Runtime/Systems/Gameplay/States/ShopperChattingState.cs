using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperChattingState : GameplayState
    {
        private IShopperSystem shopperSystem;
        private IPlayerSystem playerSystem;

        // TODO: test
        private float expiresTime;

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return;
            }

            var purchase = shopper.PopPurchaseRequest();
            var player = playerSystem.Player;

            shopper.ShowPurchase(purchase);
            player.ShowPurchase(purchase);

            expiresTime = Time.time + 2f;
        }

        protected override void OnExited(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return;
            }

            var player = playerSystem.Player;

            shopper.HidePurchase();
            player.HidePurchase();

            if (shopper.IsContainsPurchases)
            {
                return;
            }

            shopperSystem.RemoveAvailableShopper(shopper);
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            if (Time.time > expiresTime)
            {
                return Status.Completed;
            }

            // TODO: interrupt
            return Status.Working;
        }
    }
}
