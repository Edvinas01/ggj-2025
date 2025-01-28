using System;
using CHARK.GameManagement;
using PrimeTween;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ChoiceBubbleActor : MonoBehaviour, IChoiceBubbleActor
    {
        [Header("General")]
        [SerializeField]
        private Rigidbody rigidBody;

        [Header("Forces")]
        [Min(0f)]
        [SerializeField]
        private float pullForce = 5f;

        [Min(0f)]
        [SerializeField]
        private Vector2 randomForceIntervalSeconds = new(1f, 2f);

        [Min(0f)]
        [SerializeField]
        private Vector2 randomForceMagnitude = new(0.5f, 1f);

        [Header("Rendering")]
        [SerializeField]
        private Renderer imageRenderer;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        [Header("Animation")]
        [SerializeField]
        private TweenSettings scaleUpTween;

        private float nextRandomForceTimeSeconds;

        public Transform PullPoint { get; set; }

        public ItemData Item { get; private set; }

        public bool IsCorrect { get; private set; }

        public event Action OnClicked;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
            {
                return;
            }

            if (PullPoint == false)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, PullPoint.transform.position);
        }

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

        private void FixedUpdate()
        {
            if (PullPoint == false)
            {
                return;
            }

            ApplyRandomForce();
            ApplyPullForce();
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

        private void ApplyRandomForce()
        {
            if (Time.time < nextRandomForceTimeSeconds)
            {
                return;
            }

            var randomDirection = Random.onUnitSphere;
            var randomForce = randomDirection
                * Random.Range(
                    randomForceMagnitude.x,
                    randomForceMagnitude.y
                );

            rigidBody.AddForce(randomForce, ForceMode.Impulse);

            nextRandomForceTimeSeconds = Time.time
                + Random.Range(
                    randomForceIntervalSeconds.x,
                    randomForceIntervalSeconds.y
                );
        }

        private void ApplyPullForce()
        {
            var direction = (PullPoint.position - transform.position).normalized;
            var distance = Vector3.Distance(PullPoint.position, transform.position);
            var force = direction * (pullForce * Mathf.Clamp01(distance));

            rigidBody.AddForce(force);
        }
    }
}
