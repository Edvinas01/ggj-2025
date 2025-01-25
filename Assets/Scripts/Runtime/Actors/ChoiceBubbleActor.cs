using System;
using CHARK.GameManagement;
using PrimeTween;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ChoiceBubbleActor : MonoBehaviour, IChoiceBubbleActor
    {
        [Header("General")]
        [SerializeField]
        private Rigidbody rigidBody;

        [Header("Pulling")]
        [Min(0f)]
        [SerializeField]
        private float pullForce = 5f;

        [Header("Rendering")]
        [SerializeField]
        private Renderer imageRenderer;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        [Header("Animation")]
        [SerializeField]
        private TweenSettings scaleUpTween;

        public Vector3 PullPoint { get; set; }

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

            transform.localScale = Vector3.zero;
            rigidBody.linearVelocity = Vector3.zero;
        }

        private void Start()
        {
            Tween.Scale(transform, Vector3.zero, Vector3.one, scaleUpTween);
        }

        private void Update()
        {
            var dir = (PullPoint - transform.position).normalized;
            var force = dir * pullForce;
            rigidBody.AddForce(force);
        }

        public void Click()
        {
            GameManager.Publish(new ChoiceBubbleClickedMessage(this));
            OnClicked?.Invoke();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
