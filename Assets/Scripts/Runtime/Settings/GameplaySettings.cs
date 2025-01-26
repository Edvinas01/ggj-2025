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
        [Header("Entities")]
        [SerializeField]
        private List<ShopperData> availableShoppers;

        [SerializeField]
        private List<ItemData> availableItems;

        [Header("Durations")]
        [Min(0f)]
        [SerializeField]
        private Vector2 spawnDelayRange = new(1f, 3f);

        [Min(0f)]
        [SerializeField]
        private Vector2 rantDurationRange = new(1.5f, 3f);

        public IReadOnlyCollection<ShopperData> AvailableShoppers => availableShoppers;

        public IReadOnlyCollection<ItemData> AvailableItems => availableItems;

        public float SpawnDelaySeconds => Random.Range(spawnDelayRange.x, spawnDelayRange.y);

        public float RantDurationSeconds => Random.Range(rantDurationRange.x, rantDurationRange.y);
    }
}
