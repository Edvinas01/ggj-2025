﻿using System.Collections.Generic;
using System.Linq;
using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UABPetelnia.GGJ2025.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AI;

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
        private float choiceOffset = 0.2f;

        [SerializeField]
        private Vector2Int invalidItemRange = new(2, 4);

        [Header("Animations")]
        [SerializeField]
        private Animator buyAnimation;

        [SerializeField]
        private Animator punchAnimation;

        [Header("Text")]
        [SerializeField]
        private string keywordToken = "${KEYWORD}";

        [Header("Rendering")]
        [SerializeField]
        private Renderer bodyRenderer;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        private IShopperSystem shopperSystem;
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
            if (IsMoving == false)
            {
                return;
            }

            var dir = agent.destination - transform.position;
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

            var validKeywordItems = keyword.Items
                .Distinct()
                .ToList();

            var validItems = validKeywordItems
                .Shuffle()
                .Take(1)
                .ToList();

            var invalidItems = allKeywords
                .Where(k => k != keyword)
                .SelectMany(k => k.Items)
                .Where(invalidKeyword => validKeywordItems.Contains(invalidKeyword) == false)
                .Take(Random.Range(invalidItemRange.x, invalidItemRange.y))
                .Distinct()
                .Shuffle()
                .ToList();

            return new PurchaseRequest(
                text,
                invalidItems,
                validItems,
                this
            );
        }

        public void PlayBuyAnimation()
        {
            buyAnimation.gameObject.SetActive(true);
            buyAnimation.Play("Animation_Shopper_Money");
        }

        public void StopBuyAnimation()
        {
            buyAnimation.gameObject.SetActive(false);
        }

        public void PlayPunchAnimation()
        {
            punchAnimation.gameObject.SetActive(true);
            punchAnimation.Play("Animation_Shopper_Fist");
        }

        public void StopPunchAnimation()
        {
            punchAnimation.gameObject.SetActive(false);
        }

        private Transform GetOriginPoint()
        {
            for (var index = 0; index < choiceOrigin.childCount; index++)
            {
                if (Random.value > 0.5f)
                {
                    return choiceOrigin.GetChild(index);
                }
            }

            return choiceOrigin;
        }

        public void ShowPurchase(PurchaseRequest purchase)
        {
            // TODO: move spawn logic to a system and use some spawn point for bubbles
            // TODO: spawn randomly and wobble
            // TODO: something is broken when empty item list is the only one remaining, sometimes get empty bubbles

            foreach (var invalidItem in purchase.InvalidItems)
            {
                var origin = GetOriginPoint();
                var choice = Instantiate(
                    choicePrefab,
                    origin.position + GetRandomChoiceOffset(),
                    Quaternion.identity,
                    origin
                );

                choice.PullPoint = origin.position + GetRandomChoiceOffset();
                choice.Initialize(invalidItem, isCorrect: false);
                choices.Add(choice);
            }

            foreach (var validItem in purchase.ValidItems)
            {
                var origin = GetOriginPoint();
                var choice = Instantiate(
                    choicePrefab,
                    origin.position + GetRandomChoiceOffset(),
                    Quaternion.identity,
                    origin
                );

                choice.PullPoint = origin.position + GetRandomChoiceOffset();
                choice.Initialize(validItem, isCorrect: true);
                choices.Add(choice);
            }
        }

        public void HidePurchase()
        {
            foreach (var bubble in choices)
            {
                bubble.Destroy();
            }

            choices.Clear();
        }

        private Vector3 GetRandomChoiceOffset()
        {
            return new Vector3(
                Random.Range(-choiceOffset, +choiceOffset),
                Random.Range(-choiceOffset, +choiceOffset),
                Random.Range(0f, 0f)
            );
        }
    }
}