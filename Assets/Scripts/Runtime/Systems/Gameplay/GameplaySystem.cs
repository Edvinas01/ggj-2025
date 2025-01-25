using System.Collections.Generic;
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
        private readonly List<GameplayState> states = new();

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

            var purchasedState = new ShopperPurchasedState();
            var punchingState = new ShopperPunchingState();

            var chattingState = new ShopperChattingState(purchasedState, punchingState);
            var destroyState = new ShopperDestroyState();

            // 1. Spawn player and move to kiosk after spawning
            spawnState.Initialize(moveToKioskState);

            // 2. Reaching kiosk, start chatting
            moveToKioskState.Initialize(chattingState);

            // 3. Finishing the chat, success or failure (no target state needed)
            chattingState.Initialize(default);

            // 4. On success or failure, move back to spawn
            purchasedState.Initialize(moveToSpawnPointState);
            punchingState.Initialize(moveToSpawnPointState);

            // 5. Destroy on reaching the spawn point
            moveToSpawnPointState.Initialize(destroyState);

            // 6. Restart.
            destroyState.Initialize(spawnState);

            startingState = spawnState;

            states.Add(spawnState);
            states.Add(moveToKioskState);
            states.Add(chattingState);
            states.Add(moveToSpawnPointState);
            states.Add(destroyState);
            states.Add(purchasedState);
            states.Add(punchingState);
        }

        public override void OnDisposed()
        {
            foreach (var state in states)
            {
                state.Dispose();
            }
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
