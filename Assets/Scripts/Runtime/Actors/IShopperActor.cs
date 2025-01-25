using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal interface IShopperActor
    {
        public bool IsMoving { get; }

        public string Name { get; }

        public void Destroy();

        public void Move(Vector3 position);
    }
}
