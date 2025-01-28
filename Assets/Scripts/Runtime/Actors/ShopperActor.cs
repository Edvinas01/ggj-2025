using System;
using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement;
using CHARK.ScriptableAudio;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UABPetelnia.GGJ2025.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ShopperActor : MonoBehaviour, IShopperActor
    {
        [Header("General")]
        [SerializeField]
        private NavMeshAgent agent;

        [Header("Choices")]
        [SerializeField]
        private ChoiceBubbleActor choicePrefab;

        [SerializeField]
        private Transform choiceOrigin;

        [Min(0f)]
        [SerializeField]
        private float choiceSpawnOffset = 0.1f;

        [SerializeField]
        private Vector2Int invalidItemRange = new(2, 4);

        [Header("Animations")]
        [SerializeField]
        private Animator buyAnimation;

        [SerializeField]
        private Animator punchAnimation;

        [SerializeField]
        private Animator walkAnimation;

        [Header("Text")]
        [SerializeField]
        private string keywordToken = "${KEYWORD}";

        [Header("Rendering")]
        [SerializeField]
        private Renderer bodyRenderer;

        [Header("Audio")]
        [SerializeField]
        private AudioEmitter speechAudioEmitter;

        [Header("Events")]
        [SerializeField]
        public UnityEvent onPunchStart;

        [SerializeField]
        public UnityEvent onPunchStop;

        [SerializeField]
        public UnityEvent OnBuyStart;

        [SerializeField]
        public UnityEvent OnBuyStop;

        [SerializeField]
        public UnityEvent OnMoveStart;

        [SerializeField]
        public UnityEvent OnMoveStop;

        [SerializeField]
        private UnityEvent onTalkEntered;

        [SerializeField]
        private UnityEvent onTalkExited;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        private IShopperSystem shopperSystem;
        private Camera mainCamera;
        private readonly List<IChoiceBubbleActor> choices = new();

        public string Name => name;

        public ShopperData Data { get; private set; }

        public bool IsContainsPurchases => Data.PurchaseCollection.Purchases.Count > 0;

        public bool IsBuying
        {
            get
            {
                var stateInfo = buyAnimation.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Animation_Shopper_Money") && stateInfo.normalizedTime < 1f)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsPunching
        {
            get
            {
                var stateInfo = punchAnimation.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Animation_Shopper_Fist") && stateInfo.normalizedTime < 1f)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsMoving
        {
            get
            {
                var dist = agent.remainingDistance;
                return float.IsPositiveInfinity(dist)
                    || agent.pathStatus != NavMeshPathStatus.PathComplete
                    || agent.remainingDistance != 0;
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
            {
                return;
            }

            if (IsMoving == false)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, agent.destination);
        }

        private void Awake()
        {
            shopperSystem = GameManager.GetSystem<IShopperSystem>();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            shopperSystem.AddShopper(this);
        }

        private void OnDisable()
        {
            shopperSystem.RemoveShopper(this);
        }

        private void Update()
        {
            Vector3 dir;
            if (IsMoving == false)
            {
                dir = mainCamera.transform.position - transform.position;
            }
            else
            {
                dir = agent.destination - transform.position;
            }

            dir.y = 0;

            if (dir == Vector3.zero)
            {
                return;
            }

            var rot = Quaternion.LookRotation(dir);
            transform.rotation = rot;
        }

        public void Initialize(ShopperData data)
        {
            // Data is a mutable clone, so can modify
            Data = data;

            var block = new MaterialPropertyBlock();
            block.SetTexture(texturePropertyId, data.Image);
            bodyRenderer.SetPropertyBlock(block);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void Move(Vector3 position)
        {
            agent.SetDestination(position);
        }

        public PurchaseRequest PopPurchaseRequest()
        {
            var purchaseCollection = Data.PurchaseCollection;
            var purchases = purchaseCollection.Purchases;
            var purchase = purchases.GetRandom();

            var allKeywords = purchase.Keywords.ToList();
            var unusedKeywords = allKeywords.Where(k => k.IsUsed == false).ToList();
            var keyword = unusedKeywords.GetRandom();

            keyword.IsUsed = true;

            if (unusedKeywords.Count <= 1)
            {
                purchases.Remove(purchase);
            }

            var text = purchase.TemplateText.Replace(keywordToken, keyword.Text);

            if (unusedKeywords.Count(k => k.Items.Count <= 0) == unusedKeywords.Count)
            {
                return new PurchaseRequest(
                    text: text,
                    invalidItems: Array.Empty<ItemData>(),
                    validItems: Array.Empty<ItemData>(),
                    shopper: this
                );
            }

            var validKeywordItems = keyword.Items
                .Distinct()
                .ToList();

            var availableItems = shopperSystem.AvailableItems;
            var invalidItems = availableItems
                .Where(item => validKeywordItems.Contains(item) == false)
                .Shuffle()
                .Take(Random.Range(invalidItemRange.x, invalidItemRange.y))
                .ToList();

            var validItems = validKeywordItems
                .Shuffle()
                .Take(1)
                .ToList();

            return new PurchaseRequest(
                text: text,
                invalidItems: invalidItems,
                validItems: validItems,
                shopper: this
            );
        }

        public void PlayBuyAnimation()
        {
            buyAnimation.gameObject.SetActive(true);
            buyAnimation.Play("Animation_Shopper_Money");
            OnBuyStart.Invoke();
        }

        public void StopBuyAnimation()
        {
            buyAnimation.gameObject.SetActive(false);
            OnBuyStop.Invoke();
        }

        public void PlayPunchAnimation()
        {
            punchAnimation.gameObject.SetActive(true);
            punchAnimation.Play("Animation_Shopper_Fist");
            onPunchStart.Invoke();
        }

        public void StopPunchAnimation()
        {
            punchAnimation.gameObject.SetActive(false);
            onPunchStop.Invoke();
        }

        public void PlayWalkAnimation()
        {
            if (walkAnimation)
            {
                walkAnimation.enabled = true;
                walkAnimation.Play("Animation_Shooper_Walk_Jump", -1, 0f);
            }

            OnMoveStart.Invoke();
        }

        public void StopWalkAnimation()
        {
            if (walkAnimation)
            {
                walkAnimation.Play(walkAnimation.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
                walkAnimation.Update(0f);
                walkAnimation.enabled = false;
            }

            OnMoveStop.Invoke();
        }

        public void PlaySpeech()
        {
            speechAudioEmitter.Play();
        }

        public void StopSpeech()
        {
            speechAudioEmitter.Stop();
        }

        public void ShowPurchase(PurchaseRequest purchase)
        {
            var points = new Queue<Transform>(GetShuffledOriginPoints());
            foreach (var invalidItem in purchase.InvalidItems)
            {
                var origin = points.Dequeue();
                var choice = Instantiate(
                    choicePrefab,
                    choiceOrigin.transform.position + GetRandomChoiceSpawnOffset(),
                    Quaternion.identity
                );

                choice.PullPoint = origin;
                choice.Initialize(invalidItem, isCorrect: false);
                choices.Add(choice);
            }

            foreach (var validItem in purchase.ValidItems)
            {
                var origin = points.Dequeue();
                var choice = Instantiate(
                    choicePrefab,
                    choiceOrigin.transform.position + GetRandomChoiceSpawnOffset(),
                    Quaternion.identity
                );

                choice.PullPoint = origin;
                choice.Initialize(validItem, isCorrect: true);
                choices.Add(choice);
            }
        }

        public void HidePurchase()
        {
            foreach (var bubble in choices)
            {
                if (bubble is Object bubbleObject && bubbleObject)
                {
                    bubble.Destroy();
                }
            }

            choices.Clear();
        }

        private IEnumerable<Transform> GetShuffledOriginPoints()
        {
            var points = new List<Transform>();
            for (var index = 0; index < choiceOrigin.childCount; index++)
            {
                points.Add(choiceOrigin.GetChild(index));
            }

            return points.Shuffle();
        }

        private Vector3 GetRandomChoiceSpawnOffset()
        {
            return new Vector3(
                Random.Range(-choiceSpawnOffset, +choiceSpawnOffset),
                Random.Range(-choiceSpawnOffset, +choiceSpawnOffset),
                Random.Range(-choiceSpawnOffset, +choiceSpawnOffset)
            );
        }
    }
}
