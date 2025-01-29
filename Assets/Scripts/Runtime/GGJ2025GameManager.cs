using System.Collections;
using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Systems.Audio;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Gameplay;
using UABPetelnia.GGJ2025.Runtime.Systems.Input;
using UABPetelnia.GGJ2025.Runtime.Systems.Interaction;
using UABPetelnia.GGJ2025.Runtime.Systems.Pausing;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.Systems.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Shoppers;
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

        [SerializeField]
        private ShopperSystem shopperSystem;

        [SerializeField]
        private GameplaySystem gameplaySystem;

        protected override void OnBeforeInitializeSystems()
        {
            AddSystem(audioSystem);
            AddSystem(cursorSystem);
            AddSystem(inputSystem);
            AddSystem(pauseSystem);
            AddSystem(settingsSystem);
            AddSystem(sceneSystem);

            AddSystem(new PlayerSystem());
            AddSystem(shopperSystem);
            AddSystem(gameplaySystem);
            AddSystem(new InteractionSystem());
        }

        protected override void OnAfterInitializeSystems()
        {
        }

        protected override void OnStarted()
        {
            base.OnStarted();
#if UNITY_WEBGL
            StartCoroutine(LoadGameRoutine());
#else
            sceneSystem.LoadInitialScene();
#endif
        }

#if UNITY_WEBGL
        private IEnumerator LoadGameRoutine()
        {
            if (audioSystem.IsLoading)
            {
                yield return null;
            }

            // TODO: scuffed workaround for WebGL not playing audio in main menu, oh well...
            yield return new WaitForSeconds(1f);
            sceneSystem.LoadInitialScene();
        }
#endif
    }
}
