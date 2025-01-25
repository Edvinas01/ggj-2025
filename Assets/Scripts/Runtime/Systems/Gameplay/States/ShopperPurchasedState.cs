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
        }

        protected override void OnExited(GameplayStateContext context)
        {
            var item = context.CurrentItem;
            if (item == false)
            {
                return;
            }

            var player = playerSystem.Player;
            player.Cents += item.Cents;

            context.CurrentItem = default;
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            return Status.Completed;
        }
    }
}
