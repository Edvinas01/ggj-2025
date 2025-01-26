using System.Linq;
using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Settings;
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

        private float expiryTimeSeconds;
        private bool isExpires;

        private readonly GameplaySettings gameplaySettings;
        private readonly GameplayState successState;
        private readonly GameplayState failureState;

        public ShopperChattingState(GameplaySettings gameplaySettings, GameplayState successState, GameplayState failureState)
        {
            this.gameplaySettings = gameplaySettings;
            this.successState = successState;
            this.failureState = failureState;
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
            expiryTimeSeconds = 0f;

            isFinishedChatting = false;
            isExpires = false;

            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return;
            }

            var purchase = shopper.PopPurchaseRequest();

            Debug.Log("Invalid Items: " + string.Join(", ", purchase.InvalidItems.Select(i => i.name)));
            Debug.Log("Valid Items: " + string.Join(", ", purchase.ValidItems.Select(i => i.name)));

            var player = playerSystem.Player;

            if (purchase.IsEmpty)
            {
                expiryTimeSeconds = Time.time + gameplaySettings.RantDurationSeconds;
                isExpires = true;
                NextState = failureState;
                player.ShowPurchase(purchase);
                return;
            }

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
            isFinishedChatting = true;

            if (message.Bubble.IsCorrect)
            {
                Context.CurrentItem = message.Bubble.Item;
                NextState = successState;
            }
            else
            {
                Context.CurrentItem = default;
                NextState = failureState;
            }
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            if (isExpires && Time.time >= expiryTimeSeconds)
            {
                return Status.Completed;
            }

            if (isFinishedChatting)
            {
                return Status.Completed;
            }

            return Status.Working;
        }
    }
}
