using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Audio;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Input;
using UABPetelnia.GGJ2025.Runtime.Systems.Pausing;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.Systems.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime
{
    // ReSharper disable once InconsistentNaming
    internal sealed class GGJ2025GameManager : GameManager
    {
        [Header("Systems")]
        [SerializeField]
        private AudioSystem audioSystem;

        [SerializeField]
        private CursorSystem cursorSystem;

        [SerializeField]
        private InputSystem inputSystem;

        [SerializeField]
        private PauseSystem pauseSystem;

        [SerializeField]
        private SettingsSystem settingsSystem;

        [SerializeField]
        private SceneSystem sceneSystem;

        protected override void OnBeforeInitializeSystems()
        {
            AddSystem(audioSystem);
            AddSystem(cursorSystem);
            AddSystem(inputSystem);
            AddSystem(pauseSystem);
            AddSystem(settingsSystem);
            AddSystem(sceneSystem);
        }

        protected override void OnAfterInitializeSystems()
        {
        }
    }
}
