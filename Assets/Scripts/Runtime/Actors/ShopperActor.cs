using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;
using UnityEngine.AI;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ShopperActor : MonoBehaviour, IShopperActor
    {
        [Header("General")]
        [SerializeField]
        private NavMeshAgent agent;

        [Header("Rendering")]
        [SerializeField]
        private Renderer bodyRenderer;

        [SerializeField]
        private string texturePropertyId = "_BaseMap";

        private IShopperSystem shopperSystem;
        private ShopperData shopperData;

        public string Name => name;

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
            shopperData = data;

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
    }
}
