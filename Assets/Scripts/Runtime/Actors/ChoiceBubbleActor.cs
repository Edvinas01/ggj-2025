using System;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ChoiceBubbleActor : MonoBehaviour, IChoiceBubbleActor
    {
        [Header("Rendering")]
        [SerializeField]
        private Renderer imageRenderer;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        public ItemData Item { get; private set; }

        public bool IsCorrect { get; private set; }

        public event Action OnClicked;

        public void Initialize(ItemData item, bool isCorrect)
        {
            Item = item;
            IsCorrect = isCorrect;

            var block = new MaterialPropertyBlock();
            block.SetTexture(texturePropertyId, Item.Image);
            imageRenderer.SetPropertyBlock(block);
        }

        public void Click()
        {
            OnClicked?.Invoke();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
