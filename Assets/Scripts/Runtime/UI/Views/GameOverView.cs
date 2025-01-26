using System;
using CHARK.SimpleUI;
using UnityEngine;
using UnityEngine.UI;

namespace UABPetelnia.GGJ2025.Runtime.UI.Views
{
    internal sealed class GameOverView : View
    {
        [Header("Buttons")]
        [SerializeField]
        private Button restartGameButton;

        [SerializeField]
        private Button exitGameButton;

        public event Action OnRestartGameClicked;

        public event Action OnExitGameClicked;

        protected override void OnEnable()
        {
            base.OnEnable();

            restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
            exitGameButton.onClick.AddListener(OnExitGameButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            restartGameButton.onClick.RemoveListener(OnRestartGameButtonClicked);
            exitGameButton.onClick.RemoveListener(OnExitGameButtonClicked);
        }

        private void OnRestartGameButtonClicked()
        {
            OnRestartGameClicked?.Invoke();
        }

        private void OnExitGameButtonClicked()
        {
            OnExitGameClicked?.Invoke();
        }
    }
}
