﻿using CHARK.GameManagement;
using CHARK.SimpleUI;
using UABPetelnija.GGJ2025.Runtime.Settings;
using UABPetelnija.GGJ2025.Runtime.Systems.Audio;
using UABPetelnija.GGJ2025.Runtime.Systems.Input;
using UABPetelnija.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnija.GGJ2025.Runtime.UI.Views;

namespace UABPetelnija.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class MainMenuViewController : ViewController<MainMenuView>
    {
        private ISceneSystem sceneSystem;
        private IInputSystem inputSystem;
        private IAudioSystem audioSystem;

        protected override void Awake()
        {
            base.Awake();

            sceneSystem = GameManager.GetSystem<ISceneSystem>();
            inputSystem = GameManager.GetSystem<IInputSystem>();
            audioSystem = GameManager.GetSystem<IAudioSystem>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            View.OnStartGameClicked += OnViewStartGameClicked;
            View.OnExitGameClicked += OnViewExitGameClicked;

            View.OnLookSensitivityChanged += OnViewLookSensitivityChanged;
            View.OnMasterVolumeChanged += OnViewMasterVolumeChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            View.OnStartGameClicked -= OnViewStartGameClicked;
            View.OnExitGameClicked -= OnViewExitGameClicked;

            View.OnLookSensitivityChanged -= OnViewLookSensitivityChanged;
            View.OnMasterVolumeChanged -= OnViewMasterVolumeChanged;
        }

        protected override void Start()
        {
            base.Start();

            InitializeLookSensitivityData();
            InitializeMasterVolumeData();
        }

        private void OnViewStartGameClicked()
        {
            sceneSystem.LoadGameplayScene();
        }

        private void OnViewExitGameClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }

        private void OnViewLookSensitivityChanged(float sensitivity)
        {
            inputSystem.LookSensitivity = sensitivity;
        }

        private void OnViewMasterVolumeChanged(float volume)
        {
            audioSystem.SetVolume(VolumeType.Master, volume);
        }

        private void InitializeLookSensitivityData()
        {
            var lookSensitivity = inputSystem.LookSensitivity;

            View.SetLookSensitivityData(
                GeneralSettings.MinLookSensitivity,
                GeneralSettings.MaxLookSensitivity,
                lookSensitivity,
                isNotifyListeners: false
            );
        }

        private void InitializeMasterVolumeData()
        {
            var volume = audioSystem.GetVolume(VolumeType.Master);

            View.SetMasterVolumeData(
                GeneralSettings.MinVolume,
                GeneralSettings.MaxVolume,
                volume,
                isNotifyListeners: false
            );
        }
    }
}