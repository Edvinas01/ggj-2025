using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Components.Input;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Gameplay;
using UABPetelnia.GGJ2025.Runtime.Systems.Players;
using UABPetelnia.GGJ2025.Runtime.UI.Controllers;
using Unity.Cinemachine;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    internal sealed class DesktopPlayerActor : MonoBehaviour, IPlayerActor
    {
        [Header("General")]
        [SerializeField]
        private PlayerSettings settings;

        [SerializeField]
        private CinemachineCamera cinemachineCamera;

        [Header("UI")]
        [SerializeField]
        private GameplayViewController gameplayViewController;

        [Header("Input")]
        [SerializeField]
        private ButtonInputActionListener zoomInputListener;

        private IGameplaySystem gameplaySystem;
        private IPlayerSystem playerSystem;
        private ICursorSystem cursorSystem;

        private float initialFov;
        private float currentFov;
        private float targetFov;

        private void Awake()
        {
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
        }

        private void OnDisable()
        {
            playerSystem.RemovePlayer(this);

            zoomInputListener.OnPerformed -= OnZoomPerformed;
            zoomInputListener.OnCanceled -= OnZoomCanceled;
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

        private void OnZoomCanceled(bool value)
        {
            StopZoomingIn();
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
            gameplayViewController.ShowPurchase(purchase);
        }

        public void HidePurchase()
        {
            gameplayViewController.Hide();
        }
    }
}
