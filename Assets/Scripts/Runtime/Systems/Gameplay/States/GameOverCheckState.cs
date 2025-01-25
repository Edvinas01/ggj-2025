using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class GameOverCheckState : GameplayState
    {
        private IShopperSystem shopperSystem;
        private IPlayerSystem playerSystem;
        private ISceneSystem sceneSystem;

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        protected override void OnDisposed()
        {
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            if (shopperSystem.IsShoppersAvailable == false)
            {
                sceneSystem.LoadGameVictoryScene();
                return;
            }

            var player = playerSystem.Player;
            if (player.IsCentsGoalReached)
            {
                sceneSystem.LoadGameVictoryScene();
                return;
            }

            if (player.Health <= 0)
            {
                sceneSystem.LoadGameOverScene();
            }
        }

        protected override void OnExited(GameplayStateContext context)
        {
        }

        protected override Status OnUpdated(GameplayStateContext context)
        {
            return Status.Completed;
        }
    }
}
