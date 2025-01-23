using System;
using CHARK.SimpleUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UABPetelnija.GGJ2025.Runtime.UI.Views
{
    internal sealed class PauseMenuView : View
    {
        [Header("Buttons")]
        [SerializeField]
        private Button resumeButton;

        [SerializeField]
        private Button exitButton;

        public event Action OnResumeClicked;

        public event Action OnExitClicked;

        protected override void OnEnable()
        {
            base.OnEnable();

            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        protected override void OnViewShowEntered()
        {
            base.OnViewShowEntered();

            var eventSystem = EventSystem.current;
            if (eventSystem == false)
            {
                return;
            }

            eventSystem.SetSelectedGameObject(
                resumeButton.gameObject,
                new BaseEventData(eventSystem)
            );
        }

        private void OnResumeButtonClicked()
        {
            OnResumeClicked?.Invoke();
        }

        private void OnExitButtonClicked()
        {
            OnExitClicked?.Invoke();
        }
    }
}
