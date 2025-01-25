using CHARK.SimpleUI;
using TMPro;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.UI.Views
{
    internal sealed class MoneyView : View
    {
        [SerializeField]
        private TMP_Text displayText;

        public string DisplayText
        {
            set => displayText.text = value;
        }
    }
}
