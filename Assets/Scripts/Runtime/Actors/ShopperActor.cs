using System.Collections.Generic;
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

            var keywords = purchase.Keywords;
            var keyword = keywords.GetRandom();

            keywords.Remove(keyword);

            if (keywords.Count <= 0)
            {
                purchases.Remove(purchase);
            }

            var text = purchase.TemplateText.Replace(keywordToken, keyword.Text);

            var validItems = keyword.Items;
            var invalidItems = keywords
                .SelectMany(invalidKeyword => invalidKeyword.Items)
                .Where(invalidKeyword => validItems.Contains(invalidKeyword) == false)
                .ToList();

            return new PurchaseRequest(
                text,
                invalidItems,
                validItems,
                this
            );
        }

        public void ShowPurchase(PurchaseRequest purchase)
        {
            // TODO: move spawn logic to a system and use some spawn point for bubbles
            // TODO: spawn randomly and wobble
            // TODO: something is broken when empty item list is the only one remaining, sometimes get empty bubbles

            foreach (var invalidItem in purchase.InvalidItems)
            {
                var choice = Instantiate(
                    choicePrefab,
                    choiceOrigin.position
                    // TODO: scuffed offset
                    + new Vector3(
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(-0.2f, 0.2f)
                    ) - choiceOrigin.forward * 0.3f,
                    Quaternion.identity
                );

                choice.Initialize(invalidItem, isCorrect: false);
                choices.Add(choice);
            }

            foreach (var validItem in purchase.ValidItems)
            {
                var choice = Instantiate(
                    choicePrefab,
                    choiceOrigin.position
                    // TODO: scuffed offset
                    + new Vector3(
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(-0.2f, 0.2f),
                        Random.Range(-0.2f, 0.2f)
                    ) - choiceOrigin.forward * 0.3f,
                    Quaternion.identity
                );

                choice.Initialize(validItem, isCorrect: false);
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
    }
}
