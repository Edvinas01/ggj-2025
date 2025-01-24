using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal interface IShopperActor
    {
        public string Name { get; }

        public void WalkTo(Vector3 position);

        public void Destroy();
    }
}
