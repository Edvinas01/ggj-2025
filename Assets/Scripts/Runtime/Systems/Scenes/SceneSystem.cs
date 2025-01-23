using System.Collections.Generic;
using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using CHARK.ScriptableScenes;
using CHARK.ScriptableScenes.Events;
using UABPetelnija.GGJ2025.Runtime.Systems.Pausing;
using UnityEngine;

namespace UABPetelnija.GGJ2025.Runtime.Systems.Scenes
{
    internal sealed class SceneSystem : MonoSystem, ISceneSystem
    {
        [Header("General")]
        [SerializeField]
        private ScriptableSceneController controller;

        [Header("Scenes")]
        [SerializeField]
        private ScriptableSceneCollection menuSceneCollection;

        [SerializeField]
        private ScriptableSceneCollection startingSceneCollection;

        private IPauseSystem pauseSystem;

        public bool IsLoading => controller.IsLoading;

        public override void OnInitialized()
        {
            pauseSystem = GameManager.GetSystem<IPauseSystem>();

            controller.CollectionEvents.OnLoadEntered += OnLoadEntered;
            controller.CollectionEvents.OnLoadExited += OnLoadExited;
        }

        public override void OnDisposed()
        {
            controller.CollectionEvents.OnLoadEntered -= OnLoadEntered;
            controller.CollectionEvents.OnLoadExited -= OnLoadExited;
        }

        public bool TryGetLoadedCollection(out ScriptableSceneCollection collection)
        {
            return controller.TryGetLoadedSceneCollection(out collection);
        }

        public bool IsStartingScene(ScriptableSceneCollection collection)
        {
            return startingSceneCollection == collection || menuSceneCollection == collection;
        }

        public void ReloadScene()
        {
            controller.ReloadLoadedSceneCollection();
        }

        public void LoadMenuScene()
        {
            controller.LoadSceneCollection(menuSceneCollection);
        }

        public void LoadGameplayScene()
        {
            controller.LoadSceneCollection(startingSceneCollection);
        }

        public void LoadScene(ScriptableSceneCollection collection)
        {
            controller.LoadSceneCollection(collection);
        }

        private static void OnLoadEntered(CollectionLoadEventArgs args)
        {
            var message = new SceneLoadEnteredMessage(args.Collection);
            GameManager.Publish(message);
        }

        private void OnLoadExited(CollectionLoadEventArgs args)
        {
            pauseSystem.ResumeGame();

            var message = new SceneLoadExitedMessage(args.Collection);
            GameManager.Publish(message);
        }
    }
}
