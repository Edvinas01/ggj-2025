using UABPetelnia.GGJ2025.Runtime.Components.Interaction.Interactables;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Components.Interaction.Interactors
{
    internal sealed class ChoiceBubbleInteractor : RaycastInteractor
    {
        [SerializeField]
        private PlayerSettings settings;

        protected override IRaycastInteractorSettings Settings => settings;

        protected override bool IsValid(IInteractable interactable)
        {
            return interactable is ChoiceBubbleInteractable;
        }
    }
}
