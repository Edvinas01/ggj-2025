﻿using System.Collections;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Central scene controller which handles loading and unloading of
    /// <see cref="ScriptableSceneCollection"/>.
    /// </summary>
    [AddComponentMenu(
        AddComponentMenuConstants.BaseMenuName + "/Scriptable Scene Controller"
    )]
    public sealed class ScriptableSceneController : MonoBehaviour
    {
        private enum CollectionLoadMode
        {
            [Tooltip("Collection will not be loaded automatically")]
            // ReSharper disable once UnusedMember.Local
            None,

            [Tooltip("Automatically load collection in Awake() method")]
            Awake,

            [Tooltip("Automatically load collection in Start() method")]
            Start,
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#else
        [Header("General")]
#endif
        [Tooltip("Scene collection which is first to be loaded when the game runs in build mode")]
        [SerializeField]
        private ScriptableSceneCollection initialCollection;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#endif
        [Tooltip("Should and when " + nameof(initialCollection) + " be loaded?")]
        [FormerlySerializedAs("initialSceneLoadMode")]
        [SerializeField]
        private CollectionLoadMode initialCollectionLoadMode = CollectionLoadMode.Start;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Collection Events")]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#else
        [Header("Events")]
#endif
        [Tooltip("handler for global (invoked for all collections) collection events")]
        [SerializeField]
        private CollectionEventHandler collectionEvents = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Scene Events")]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [Tooltip("Handler for global (invoked for all scenes) scene events")]
        [SerializeField]
        private SceneEventHandler sceneEvents = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Debug")]
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        private ScriptableSceneCollection loadedCollection;

        /// <summary>
        /// Global events invoked for all <see cref="ScriptableSceneCollection"/> assets.
        /// </summary>
        public ICollectionEventHandler CollectionEvents => collectionEvents;

        /// <summary>
        /// Global events invoked for all <see cref="ScriptableScene"/> assets.
        /// </summary>
        public ISceneEventHandler SceneEvents => sceneEvents;

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// <c>true</c> if there currently a collection being loaded in
        /// <see cref="LoadingCollection"/> or <c>false</c> otherwise.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Currently loading collection.
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Debug")]
        [Sirenix.OdinInspector.ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        private ScriptableSceneCollection LoadingCollection { get; set; }

        private void Awake()
        {
            var otherController = FindFirstObjectByType<ScriptableSceneController>();
            if (otherController && otherController != this)
            {
                var controllerGameObject = otherController.gameObject;
                var controllerScene = controllerGameObject.scene;

                Debug.LogError(
                    ""
                    + $"Only one {nameof(ScriptableSceneController)} may exists."
                    + $" Scene \"{controllerScene.name}\" already contains one",
                    this
                );

                enabled = false;
                return;
            }

            if (initialCollectionLoadMode != CollectionLoadMode.Awake)
            {
                return;
            }

            LoadInitialSceneCollection();
        }

        private void Start()
        {
            if (initialCollectionLoadMode != CollectionLoadMode.Start)
            {
                return;
            }

            LoadInitialSceneCollection();
        }

        /// <summary>
        /// Load the initial Scene Collection specified in <see cref="initialCollection"/>.
        /// </summary>
        public void LoadInitialSceneCollection()
        {
#if UNITY_EDITOR
            LoadSelectedOrOpenedCollectionEditor();
#else
            LoadInitialSceneCollectionBuild();
#endif
        }

        /// <summary>
        /// Reloads <see cref="loadedCollection"/>.
        /// </summary>
        public void ReloadLoadedSceneCollection()
        {
            if (loadedCollection == false)
            {
                Debug.LogWarning(
                    $"No {nameof(ScriptableSceneCollection)} is loaded, reload will be ignored",
                    this
                );

                return;
            }

            LoadSceneCollection(loadedCollection);
        }

        /// <summary>
        /// Load a set of scenes using the provided <paramref name="collection"/> and unload
        /// <see cref="loadedCollection"/>.
        /// </summary>
        public void LoadSceneCollection(ScriptableSceneCollection collection, bool isFirstLoad = false)
        {
            StartCoroutine(LoadSceneCollectionRoutine(collection, isFirstLoad: isFirstLoad));
        }

        /// <returns>
        /// <c>true</c> if a collection which is currently being loaded is retrieved or
        /// <c>false</c> otherwise.
        /// </returns>
        public bool TryGetLoadingSceneCollection(out ScriptableSceneCollection collection)
        {
            collection = LoadingCollection;
            return collection != false;
        }

        /// <returns>
        /// <c>true</c> if a collection which is currently loaded is retrieved or <c>false</c>
        /// otherwise.
        /// </returns>
        public bool TryGetLoadedSceneCollection(out ScriptableSceneCollection collection)
        {
            collection = loadedCollection;
            return collection != false;
        }

#if UNITY_EDITOR
        private void LoadSelectedOrOpenedCollectionEditor()
        {
            StartCoroutine(LoadSelectedOrOpenedCollectionRoutineEditor());
        }

        private IEnumerator LoadSelectedOrOpenedCollectionRoutineEditor()
        {
            if (ScriptableSceneUtilities.TryGetSelectedCollection(out var selected))
            {
                yield return LoadSceneCollectionRoutine(selected, isFirstLoad: true);
                yield break;
            }

            if (ScriptableSceneUtilities.TryGetOpenCollection(out var open))
            {
                yield return LoadSceneCollectionRoutine(open, isFirstLoad: true);
                yield break;
            }

            Debug.LogWarning(
                $"Cannot load initial {nameof(ScriptableSceneCollection)}, make sure a valid " +
                $"{nameof(ScriptableSceneCollection)} exists which matches currently loaded " +
                $"scenes, or use the Scene Manager Window",
                this
            );
        }
#else
        private void LoadInitialSceneCollectionBuild()
        {
            StartCoroutine(LoadInitialSceneCollectionRoutineBuild());
        }

        private IEnumerator LoadInitialSceneCollectionRoutineBuild()
        {
            if (initialCollection == false)
            {
                Debug.LogWarning(
                    $"{nameof(initialCollection)} is not set, initial scene setup will not be " +
                    $"loaded",
                    this
                );

                yield break;
            }

            yield return LoadSceneCollectionRoutine(initialCollection, isFirstLoad: true);
        }
#endif

        private IEnumerator LoadSceneCollectionRoutine(ScriptableSceneCollection collection, bool isFirstLoad)
        {
            if (collection.SceneCount == 0)
            {
                Debug.LogWarning(
                    $"Collection \"{collection.Name}\" does not contain any scenes! Load will be " +
                    $"ignored",
                    this
                );

                yield break;
            }

            if (IsLoading)
            {
                Debug.LogWarning(
                    $"Can't load two collections at the same time, collection " +
                    $"\"{LoadingCollection.Name}\" is currently being loaded",
                    this
                );

                yield break;
            }

            LoadingCollection = collection;
            IsLoading = true;

            try
            {
                yield return LoadSceneCollectionInternalRoutine(collection, isFirstLoad: isFirstLoad);
                loadedCollection = collection;
            }
            finally
            {
                LoadingCollection = null;
                IsLoading = false;
            }
        }

        private IEnumerator LoadSceneCollectionInternalRoutine(
            ScriptableSceneCollection collection,
            bool isFirstLoad
        )
        {
            // TODO: reduce nesting
            try
            {
                collection.CollectionEvents.AddTransitionListeners(collectionEvents);
                yield return collection.ShowTransitionRoutine(isFirstLoad: isFirstLoad);

                if (loadedCollection != false)
                {
                    try
                    {
                        loadedCollection.CollectionEvents.AddListeners(collectionEvents);
                        loadedCollection.SceneEvents.AddListeners(sceneEvents);
                        yield return loadedCollection.UnloadRoutine();
                    }
                    finally
                    {
                        loadedCollection.CollectionEvents.RemoveListeners(collectionEvents);
                        loadedCollection.SceneEvents.RemoveListeners(sceneEvents);
                    }
                }

                try
                {
                    collection.CollectionEvents.AddListeners(collectionEvents);
                    collection.SceneEvents.AddListeners(sceneEvents);
                    yield return collection.LoadRoutine();

                    // TODO: scuffed
                    LightProbes.TetrahedralizeAsync();
                }
                finally
                {
                    collection.CollectionEvents.RemoveListeners(collectionEvents);
                    collection.SceneEvents.RemoveListeners(sceneEvents);
                }

                yield return collection.DelayTransitionRoutine();
                yield return collection.HideTransitionRoutine(isFirstLoad: isFirstLoad);
            }
            finally
            {
                collection.CollectionEvents.RemoveTransitionListeners(collectionEvents);
            }
        }
    }
}
