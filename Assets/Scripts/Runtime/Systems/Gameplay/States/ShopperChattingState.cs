using System.Linq;
using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperChattingState : GameplayState
    {
        private IShopperSystem shopperSystem;
        private IPlayerSystem playerSystem;
        private bool isFinishedChatting;

        private readonly GameplayState successState;
        private readonly GameplayState failreState;

        public ShopperChattingState(GameplayState successState, GameplayState failreState)
        {
            this.successState = successState;
            this.failreState = failreState;
        }

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
            playerSystem = GameManager.GetSystem<IPlayerSystem>();

            GameManager.AddListener<ChoiceBubbleClickedMessage>(OnChoiceBubbleClicked);
        }

        protected override void OnDisposed()
        {
            GameManager.RemoveListener<ChoiceBubbleClickedMessage>(OnChoiceBubbleClicked);
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            isFinishedChatting = false;

            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return;
            }

            var purchase = shopper.PopPurchaseRequest();

            Debug.Log("Invalid Items: " + string.Join(", ", purchase.InvalidItems.Select(i => i.name)));
            Debug.Log("Valid Items: " + string.Join(", ", purchase.ValidItems.Select(i => i.name)));

            var player = playerSystem.Player;

            shopper.ShowPurchase(purchase);
            player.ShowPurchase(purchase);
        }

        protected override void OnExited(GameplayStateContext context)
        {
            isFinishedChatting = true;

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

        private void OnChoiceBubbleClicked(ChoiceBubbleClickedMessage message)
        {
            if (message.Bubble.IsCorrect)
            {
                NextState = successState;
            }
            else
            {
                NextState = failreState;
            }

            isFinishedChatting = true;
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            if (isFinishedChatting)
            {
                return Status.Completed;
            }

            return Status.Working;
        }
    }
}
