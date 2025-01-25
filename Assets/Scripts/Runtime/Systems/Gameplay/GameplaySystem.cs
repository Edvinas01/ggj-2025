using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay
{
    internal sealed class GameplaySystem : SimpleSystem, IGameplaySystem
    {
        private IShopperSystem shopperSystem;

        private readonly GameplayStateContext context = new();

        private GameplayState startingState;
        private GameplayState currentState;

        private GameplayState State
        {
            get => currentState;
            set
            {
                var oldState = currentState;
                var newState = value;

                if (oldState != null && oldState == newState)
                {
                    return;
                }

                oldState?.Exit(context);
                currentState = newState;
                newState?.Enter(context);

                OnStateChanged(oldState, newState);
            }
        }

        public override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();

            var spawnState = new ShopperSpawnState();
            var moveToKioskState = new ShopperMoveState(ShopperMoveState.MoveTo.KioskPoint);
            var moveToSpawnPointState = new ShopperMoveState(ShopperMoveState.MoveTo.SpawnPoint);
            var chattingState = new ShopperChattingState();
            var destroyState = new ShopperDestroyState();

            // 1. Spawn player and move to kiosk after spawning
            spawnState.Initialize(moveToKioskState);

            // 2. Reaching kiosk, start chatting
            moveToKioskState.Initialize(chattingState);

            // 3. Finishing the chat, move back to spawn point
            chattingState.Initialize(moveToSpawnPointState);

            // 4. Destroy on reaching the spawn point
            moveToSpawnPointState.Initialize(destroyState);

            // 5. Restart.
            destroyState.Initialize(spawnState);

            startingState = spawnState;
        }

        public void OnUpdated(float deltaTime)
        {
            if (State == default)
            {
                return;
            }

            if (shopperSystem.IsShoppersAvailable == false)
            {
                // TODO: move victory state
                return;
            }

            State = currentState.Update(context);
        }

        public void StartGameplay()
        {
            State = startingState;
        }

        private static void OnStateChanged(GameplayState oldState, GameplayState newState)
        {
            Debug.Log($"New state {newState?.Name}");
        }
    }
}
