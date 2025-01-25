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

    internal sealed class PlayerCentsChanged : IMessage
    {
        public IPlayerActor Player { get; }

        public PlayerCentsChanged(IPlayerActor player)
        {
            Player = player;
        }
    }

    internal sealed class PlayerHealthChanged : IMessage
    {
        public IPlayerActor Player { get; }

        public PlayerHealthChanged(IPlayerActor player)
        {
            Player = player;
        }
    }
}
