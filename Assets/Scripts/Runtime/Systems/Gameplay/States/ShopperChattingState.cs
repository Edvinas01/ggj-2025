using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperChattingState : GameplayState
    {
        private IPlayerSystem playerSystem;

        protected override void OnInitialized()
        {
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
        }

        protected override void OnEntered(GameplayStateContext context)
        {
        }

        protected override void OnExited(GameplayStateContext context)
        {
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return Status.Completed;
            }

            var player = playerSystem.Player;
            player.ShowText($"Chatting to {shopper.Name}");

            // TODO: interrupt
            return Status.Working;
        }
    }
}
