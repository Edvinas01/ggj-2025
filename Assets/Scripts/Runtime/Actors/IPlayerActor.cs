namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal interface IPlayerActor
    {
        public int Health { get; set; }

        public int Cents { get; set; }

        public void ShowPurchase(PurchaseRequest text);

        public void HidePurchase();
    }
}
