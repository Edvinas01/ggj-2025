using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay
{
    internal sealed class GameplaySystem : SimpleSystem, IGameplaySystem
    {
        private enum GameplayState
        {
            Stopped,
            SpawningShopper,
            ShopperEntering,
            ShopperChatting,
            ShopperLeaving,
            DeSpawningShopper,
            GameLost,
            GameWon,
        }

        private IShopperSystem shopperSystem;
        private IPlayerSystem playerSystem;

        private GameplayState currentState = GameplayState.Stopped;

        private GameplayState State
        {
            set
            {
                var oldState = currentState;
                var newState = value;

                if (oldState == newState)
                {
                    return;
                }

                currentState = newState;

                OnStateChanged(oldState, newState);
            }
        }

        public override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
        }

        public void StartGameplay()
        {
            State = GameplayState.SpawningShopper;
        }

        private IShopperActor activeShopper;

        private void OnStateChanged(GameplayState oldState, GameplayState newState)
        {
            Debug.Log($"Game State: {newState}");

            switch (newState)
            {
                case GameplayState.Stopped:
                {
                    break;
                }
                case GameplayState.SpawningShopper:
                {
                    activeShopper = shopperSystem.SpawnRandomShopper();
                    // TODO: set move position

                    State = GameplayState.ShopperEntering;
                    break;
                }
                case GameplayState.ShopperEntering:
                {
                    // TODO: move shopper
                    // TODO: add delay
                    break;
                }
                case GameplayState.ShopperChatting:
                {
                    var player = playerSystem.Player;
                    player.ShowText($"New shopper {activeShopper.Name}");
                    // TODO: wait until decision

                    break;
                }
                case GameplayState.ShopperLeaving:
                {
                    var player = playerSystem.Player;
                    player.HideText();

                    // TODO: move shopper
                    // TODO: add delay

                    break;
                }
                case GameplayState.DeSpawningShopper:
                {
                    activeShopper.Destroy();
                    activeShopper = default;

                    break;
                }
                case GameplayState.GameLost:
                {
                    // TODO: lose screen
                    break;
                }
                case GameplayState.GameWon:
                {
                    // TODO: win screen
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}
