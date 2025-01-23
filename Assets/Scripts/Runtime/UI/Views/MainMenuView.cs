using System;
using CHARK.SimpleUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UABPetelnija.GGJ2025.Runtime.UI.Views
{
    internal sealed class MainMenuView : View
    {
        [Header("Buttons")]
        [SerializeField]
        private Button startGameButton;

        [SerializeField]
        private Button exitGameButton;

        [Header("Sliders")]
        [SerializeField]
        private Slider lookSensitivitySlider;

        [SerializeField]
        private Slider masterVolumeSlider;

        public event Action OnStartGameClicked;

        public event Action OnExitGameClicked;

        public event Action<float> OnLookSensitivityChanged;

        public event Action<float> OnMasterVolumeChanged;

        protected override void OnEnable()
        {
            base.OnEnable();

            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            exitGameButton.onClick.AddListener(OnExitGameButtonClicked);

            lookSensitivitySlider.onValueChanged.AddListener(OnLookSensitivitySliderChanged);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            exitGameButton.onClick.AddListener(OnExitGameButtonClicked);

            lookSensitivitySlider.onValueChanged.AddListener(OnLookSensitivitySliderChanged);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderChanged);
        }

        public void SelectStartGameButton()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == false)
            {
                return;
            }

            eventSystem.SetSelectedGameObject(
                startGameButton.gameObject,
                new BaseEventData(eventSystem)
            );
        }

        public void SetLookSensitivityData(
            float min,
            float max,
            float value,
            bool isNotifyListeners = true
        )
        {
            lookSensitivitySlider.minValue = min;
            lookSensitivitySlider.maxValue = max;

            if (isNotifyListeners)
            {
                lookSensitivitySlider.value = value;
            }
            else
            {
                lookSensitivitySlider.SetValueWithoutNotify(value);
            }
        }

        public void SetMasterVolumeData(
            float min,
            float max,
            float value,
            bool isNotifyListeners = true
        )
        {
            masterVolumeSlider.minValue = min * 100f;
            masterVolumeSlider.maxValue = max * 100f;

            var percentage = value * 100f;
            if (isNotifyListeners)
            {
                masterVolumeSlider.value = percentage;
            }
            else
            {
                masterVolumeSlider.SetValueWithoutNotify(percentage);
            }
        }

        private void OnStartGameButtonClicked()
        {
            OnStartGameClicked?.Invoke();
        }

        private void OnExitGameButtonClicked()
        {
            OnExitGameClicked?.Invoke();
        }

        private void OnLookSensitivitySliderChanged(float value)
        {
            OnLookSensitivityChanged?.Invoke(value);
        }

        private void OnMasterVolumeSliderChanged(float value)
        {
            OnMasterVolumeChanged?.Invoke(value / 100f);
        }
    }
}
