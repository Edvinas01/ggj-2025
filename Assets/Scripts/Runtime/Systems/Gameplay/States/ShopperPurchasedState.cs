using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperPurchasedState : GameplayState
    {
        private IPlayerSystem playerSystem;

        protected override void OnInitialized()
        {
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
        }

        protected override void OnDisposed()
        {
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            var player = playerSystem.Player;

            shopper?.PlayBuyAnimation();

            if (context.CurrentItem)
            {
                player.PlayGiveAnimation(context.CurrentItem);
            }
        }

        protected override void OnExited(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            var player = playerSystem.Player;

            shopper?.StopBuyAnimation();
            player.StopGiveAnimation();

            var item = context.CurrentItem;
            if (item == false)
            {
                return;
            }


            player.Cents += item.Cents;

            context.CurrentItem = default;
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return Status.Completed;
            }

            if (shopper.IsBuying)
            {
                return Status.Working;
            }

            return Status.Completed;
        }
    }
}
