﻿using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ItemData),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Item Data",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class ItemData : ScriptableObject
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private int price;

        [SerializeField]
        private Texture2D image;

        public string Id => id;

        public int Cents => price;

        public Texture2D Image => image;
    }
}