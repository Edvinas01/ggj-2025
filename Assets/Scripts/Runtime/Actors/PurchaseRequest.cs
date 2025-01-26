using System.Collections.Generic;
using UABPetelnia.GGJ2025.Runtime.Settings;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class PurchaseRequest
    {
        public string Text { get; }

        public IReadOnlyCollection<ItemData> InvalidItems { get; }

        public IReadOnlyCollection<ItemData> ValidItems { get; }

        public IShopperActor Shopper { get; }

        public bool IsEmpty => InvalidItems.Count == 0 && ValidItems.Count == 0;

        public PurchaseRequest(
            string text,
            IReadOnlyCollection<ItemData> invalidItems,
            IReadOnlyCollection<ItemData> validItems,
            IShopperActor shopper
        )
        {
            Text = text;
            InvalidItems = invalidItems;
            ValidItems = validItems;
            Shopper = shopper;
        }
    }
}
