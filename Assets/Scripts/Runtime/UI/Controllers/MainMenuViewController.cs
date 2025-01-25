using CHARK.GameManagement;
using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Audio;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Input;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class MainMenuViewController : ViewController<MainMenuView>
    {
        private ICursorSystem cursorSystem;
        private ISceneSystem sceneSystem;
        private IInputSystem inputSystem;
        private IAudioSystem audioSystem;

        protected override void Awake()
        {
            base.Awake();

            cursorSystem = GameManager.GetSystem<ICursorSystem>();
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

            cursorSystem.UnLockCursor();
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
