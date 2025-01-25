using System;
using System.Collections.Generic;
using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(PurchaseCollection),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Purchase Collection",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class PurchaseCollection : ScriptableObject
    {
        [Serializable]
        internal sealed class Purchase
        {
            [Multiline(lines: 3)]
            [SerializeField]
            private string templateText;

            [SerializeField]
            private List<Keyword> keywords;

            public string TemplateText => templateText;

            public List<Keyword> Keywords => keywords;
        }

        [SerializeField]
        private List<Purchase> purchases;

        public List<Purchase> Purchases => purchases;

        public PurchaseCollection Copy()
        {
            return Instantiate(this);
        }
    }
}
