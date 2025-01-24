using CHARK.SimpleUI;
using TMPro;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.UI.Views
{
    internal sealed class GameplayView : View
    {
        [Header("Text")]
        [SerializeField]
        private TMP_Text chatText;

        public string ChatText
        {
            set => chatText.text = value;
        }
    }
}
