using CHARK.GameManagement.Messaging;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ChoiceBubbleClickedMessage : IMessage
    {
        public IChoiceBubbleActor Bubble { get; }

        public ChoiceBubbleClickedMessage(IChoiceBubbleActor bubble)
        {
            Bubble = bubble;
        }
    }
}
