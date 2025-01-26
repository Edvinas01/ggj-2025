using System.Collections.Generic;
using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay
{
    internal sealed class GameplaySystem : MonoSystem, IGameplaySystem
    {
        [SerializeField]
        private GameplaySettings gameplaySettings;

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
            GameManager.AddListener<SceneLoadEnteredMessage>(OnSceneLoadEntered);
            GameManager.AddListener<SceneUnloadEnteredMessage>(OnSceneUnloadEntered);
        }

        public override void OnDisposed()
        {
            GameManager.RemoveListener<SceneLoadEnteredMessage>(OnSceneLoadEntered);
            GameManager.RemoveListener<SceneUnloadEnteredMessage>(OnSceneUnloadEntered);
        }

        public void OnUpdated(float deltaTime)
        {
            if (State == default)
            {
                return;
            }

            State = State.Update(context);
        }

        public void StartGameplay()
        {
            InitializeStateMachine();
        }

        private static void OnStateChanged(GameplayState oldState, GameplayState newState)
        {
            Debug.Log($"New state {newState?.Name}");
        }


        private void OnSceneLoadEntered(SceneLoadEnteredMessage message)
        {
            // InitializeStateMachine();
        }

        private void OnSceneUnloadEntered(SceneUnloadEnteredMessage message)
        {
            CleanupStateMachine();
        }

        private void InitializeStateMachine()
        {
            var spawnState = new ShopperSpawnState(gameplaySettings);
            var moveToKioskState = new ShopperMoveState(ShopperMoveState.MoveTo.KioskPoint);
            var moveToSpawnPointState = new ShopperMoveState(ShopperMoveState.MoveTo.SpawnPoint);

            var purchasedState = new ShopperPurchasedState();
            var punchingState = new ShopperPunchingState();

            var chattingState = new ShopperChattingState(purchasedState, punchingState);
            var destroyState = new ShopperDestroyState();

            var gameOverCheckState = new GameOverCheckState();

            // 1. Spawn player and move to kiosk after spawning
            spawnState.Initialize(moveToKioskState);

            // 2. Reaching kiosk, start chatting
            moveToKioskState.Initialize(chattingState);

            // 3. Finishing the chat, success or failure (no target state needed)
            chattingState.Initialize(default);

            // 4. On success or failure, move back to spawn
            purchasedState.Initialize(gameOverCheckState);
            punchingState.Initialize(gameOverCheckState);

            // 5. Move to spawn point if passing game over
            gameOverCheckState.Initialize(moveToSpawnPointState);

            // 6. Destroy on reaching the spawn point
            moveToSpawnPointState.Initialize(destroyState);

            // 7. Restart.
            destroyState.Initialize(spawnState);

            State = spawnState;

            // Tracking for cleanup
            states.Clear();
            states.Add(spawnState);
            states.Add(moveToKioskState);
            states.Add(chattingState);
            states.Add(moveToSpawnPointState);
            states.Add(destroyState);
            states.Add(purchasedState);
            states.Add(punchingState);
            states.Add(gameOverCheckState);
        }

        private void CleanupStateMachine()
        {
            foreach (var state in states)
            {
                state.Dispose();
            }

            states.Clear();

            State = default;
        }
    }
}
