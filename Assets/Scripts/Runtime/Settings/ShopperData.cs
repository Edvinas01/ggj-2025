using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ShopperData),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Shopper Data",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class ShopperData : ScriptableObject
    {
        [SerializeField]
        private ShopperActor shopperPrefab;

        public ShopperActor ShopperPrefab => shopperPrefab;
    }
}
