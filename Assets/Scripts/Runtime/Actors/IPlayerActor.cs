using UABPetelnia.GGJ2025.Runtime.Settings;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal interface IPlayerActor
    {
        public int Health { get; set; }

        public int Cents { get; set; }

        public bool IsCentsGoalReached { get; }

        public void ShowPurchase(PurchaseRequest text);

        public void PlayGiveAnimation(ItemData item);

        public void StopGiveAnimation();

        public void HidePurchase();
    }
}
