using System;
using UABPetelnia.GGJ2025.Runtime.Settings;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal interface IChoiceBubbleActor
    {
        public ItemData Item { get; }

        public bool IsCorrect { get; }

        public event Action OnClicked;

        public void Initialize(ItemData item, bool isCorrect);

        public void Click();

        public void Destroy();
    }
}
