using CHARK.GameManagement;
using UABPetelnija.GGJ2025.Runtime.Systems.Audio;
using UABPetelnija.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnija.GGJ2025.Runtime.Systems.Input;
using UABPetelnija.GGJ2025.Runtime.Systems.Pausing;
using UABPetelnija.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnija.GGJ2025.Runtime.Systems.Settings;
using UnityEngine;

namespace UABPetelnija.GGJ2025.Runtime
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
