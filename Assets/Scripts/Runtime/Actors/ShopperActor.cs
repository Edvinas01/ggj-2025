using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
using UnityEngine;
using UnityEngine.AI;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class ShopperActor : MonoBehaviour, IShopperActor
    {
        [SerializeField]
        private NavMeshAgent agent;

        private IShopperSystem shopperSystem;

        private Vector3 moveDestination;

        public string Name => name;

        public bool IsMoving
        {
            get
            {
                float dist=agent.remainingDistance;
                if (!float.IsPositiveInfinity(dist) && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
                {
                    return false;
                }

                return true;
            }
        }

        private void OnDrawGizmos()
        {
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
