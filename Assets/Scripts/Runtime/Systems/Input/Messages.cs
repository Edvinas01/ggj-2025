﻿using CHARK.GameManagement.Messaging;

namespace UABPetelnija.GGJ2025.Runtime.Systems.Input
{
    internal readonly struct ControlSchemeChangedMessage : IMessage
    {
        public ControlScheme ControlScheme { get; }

        public ControlSchemeChangedMessage(ControlScheme controlScheme)
        {
            ControlScheme = controlScheme;
        }
    }
}