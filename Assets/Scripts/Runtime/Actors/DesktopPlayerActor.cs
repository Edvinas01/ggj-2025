using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Components.Input;
using UABPetelnia.GGJ2025.Runtime.Components.Interaction.Interactors;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Gameplay;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.UI.Controllers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class DesktopPlayerActor : MonoBehaviour, IPlayerActor
    {
        [Header("General")]
        [SerializeField]
        private PlayerSettings settings;

        [SerializeField]
        private CinemachineCamera cinemachineCamera;

        [SerializeField]
        private Interactor choiceInteractor;

        [Header("Rendering")]
        [SerializeField]
        private Renderer bodyRenderer;

        [SerializeField]
        private string bodyTexturePropertyId = "_BaseMap";

        [Header("UI")]
        [FormerlySerializedAs("gameplayViewController")]
        [SerializeField]
        private ChatViewController chatViewController;

        [Header("Input")]
        [SerializeField]
        private ButtonInputActionListener zoomInputListener;

        [SerializeField]
        private ButtonInputActionListener selectListener;

        private IGameplaySystem gameplaySystem;
        private IPlayerSystem playerSystem;
        private ICursorSystem cursorSystem;

        private float initialFov;
        private float currentFov;
        private float targetFov;

        private int currentHealth;
        private int currentCents;

        public int Health
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                currentHealth = Mathf.Max(currentHealth, 0);

                OnCurrentHealthChanged();
            }
        }

        public int Cents
        {
            get => currentCents;
            set
            {
                currentCents = value;
                GameManager.Publish(new PlayerCentsChanged(this));
            }
        }

        private void Awake()
        {
            currentHealth = settings.MaxHealth;

            gameplaySystem = GameManager.GetSystem<IGameplaySystem>();
            playerSystem = GameManager.GetSystem<IPlayerSystem>();
            cursorSystem = GameManager.GetSystem<ICursorSystem>();
        }

        private void Start()
        {
            initialFov = cinemachineCamera.Lens.FieldOfView;
            targetFov = cinemachineCamera.Lens.FieldOfView;

            gameplaySystem.StartGameplay();
            cursorSystem.LockCursor();
        }

        private void OnEnable()
        {
            playerSystem.AddPlayer(this);

            zoomInputListener.OnPerformed += OnZoomPerformed;
            zoomInputListener.OnCanceled += OnZoomCanceled;

            selectListener.OnPerformed += OnSelectPerformed;
            selectListener.OnCanceled += OnSelectCanceled;
        }

        private void OnDisable()
        {
            playerSystem.RemovePlayer(this);

            zoomInputListener.OnPerformed -= OnZoomPerformed;
            zoomInputListener.OnCanceled -= OnZoomCanceled;

            selectListener.OnPerformed -= OnSelectPerformed;
            selectListener.OnCanceled -= OnSelectCanceled;
        }

        private void Update()
        {
            UpdateCameraZoom();
        }

        private void UpdateCameraZoom()
        {
            currentFov = Mathf.Lerp(
                currentFov,
                targetFov,
                Time.deltaTime * settings.ZoomInSpeed
            );

            cinemachineCamera.Lens.FieldOfView = currentFov;
        }


        private void OnZoomPerformed(bool value)
        {
            StartZoomingIn();
        }

        private void OnSelectPerformed(bool value)
        {
            choiceInteractor.Select();
        }

        private void OnSelectCanceled(bool value)
        {
            choiceInteractor.Deselect();
        }

        private void OnZoomCanceled(bool value)
        {
            StopZoomingIn();
        }

        private void OnCurrentHealthChanged()
        {
            var texture = settings.GetHealthTexture(Health);
            Debug.Log(texture);

            var block = new MaterialPropertyBlock();
            block.SetTexture(bodyTexturePropertyId, texture);
            bodyRenderer.SetPropertyBlock(block);

            Debug.Log($"Health: {Health}", this);

            GameManager.Publish(new PlayerHealthChanged(this));
        }

        private void StartZoomingIn()
        {
            targetFov = settings.ZoomInFov;
        }

        private void StopZoomingIn()
        {
            targetFov = initialFov;
        }

        public void ShowPurchase(PurchaseRequest purchase)
        {
            chatViewController.ShowPurchase(purchase);
        }

        public void HidePurchase()
        {
            chatViewController.Hide();
        }
    }
}
