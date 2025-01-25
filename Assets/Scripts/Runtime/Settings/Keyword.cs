using System;
using System.Collections.Generic;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [Serializable]
    internal sealed class Keyword
    {
        [SerializeField]
        private string text;

        [SerializeField]
        private List<ItemData> items;

        public string Text => text;

        public List<ItemData> Items => items;
    }
}
