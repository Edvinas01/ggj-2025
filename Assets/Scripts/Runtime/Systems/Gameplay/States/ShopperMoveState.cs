using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Gameplay.States
{
    internal sealed class ShopperMoveState : GameplayState
    {
        public enum MoveTo
        {
            SpawnPoint,
            KioskPoint,
        }

        private readonly MoveTo moveTo;
        private IShopperSystem shopperSystem;

        public ShopperMoveState(MoveTo moveTo)
        {
            this.moveTo = moveTo;
        }

        protected override void OnInitialized()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
        }

        protected override void OnEntered(GameplayStateContext context)
        {
            var shopper = context.ActiveShopper;
            if (shopper == default)
            {
                return;
            }

            switch (moveTo)
            {
                case MoveTo.SpawnPoint:
                {
                    shopper.Move(shopperSystem.RandomSpawnPoint);
                    break;
                }
                case MoveTo.KioskPoint:
                {
                    shopper.Move(shopperSystem.KioskPoint);
                    break;
                }
            }
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

            if (shopper.IsMoving)
            {
                return Status.Working;
            }

            return Status.Completed;
        }
    }
}
