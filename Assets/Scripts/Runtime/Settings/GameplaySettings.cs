using System.Collections.Generic;
using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(GameplaySettings),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Gameplay Settings",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class GameplaySettings : ScriptableObject
    {
        [SerializeField]
        private List<ShopperData> availableShoppers;

        public IReadOnlyCollection<ShopperData> AvailableShoppers => availableShoppers;
    }
}
