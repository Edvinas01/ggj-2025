using System;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay
{
    internal abstract class GameplayState
    {
        protected enum Status
        {
            Completed,
            Working,
        }

        private GameplayState targetState;

        public string Name => GetType().Name;

        public void Initialize(GameplayState nextState)
        {
            OnInitialized();
            targetState = nextState;
        }

        public void Dispose()
        {
            OnDisposed();
        }

        public void Enter(GameplayStateContext context)
        {
            OnEntered(context);
        }

        public void Exit(GameplayStateContext context)
        {
            OnExited(context);
        }

        public GameplayState Update(GameplayStateContext context)
        {
            var status = OnUpdated(context);
            return status switch
            {
                Status.Working => this,
                Status.Completed => targetState,
                _ => throw new ArgumentOutOfRangeException($"Unsuported status: {status}")
            };
        }

        protected abstract void OnInitialized();

        protected abstract void OnDisposed();

        protected abstract void OnEntered(GameplayStateContext context);

        protected abstract void OnExited(GameplayStateContext context);

        protected abstract Status OnUpdated(GameplayStateContext context);
    }
}
