using CHARK.GameManagement;
using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Components.Input;
using UABPetelnia.GGJ2025.Runtime.Systems.Cursors;
using UABPetelnia.GGJ2025.Runtime.Systems.Pausing;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.UI.Views;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class PauseMenuViewController : ViewController<PauseMenuView>
    {
        [Header("Input Listeners")]
        [SerializeField]
        private ButtonInputActionListener toggleMenuListener;

        private ICursorSystem cursorSystem;
        private IPauseSystem pauseSystem;
        private ISceneSystem sceneSystem;

        protected override void Awake()
        {
            base.Awake();

            cursorSystem = GameManager.GetSystem<ICursorSystem>();
            pauseSystem = GameManager.GetSystem<IPauseSystem>();
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            toggleMenuListener.OnPerformed += OnToggleMenuPerformed;

            View.OnResumeClicked += OnViewResumeClicked;
            View.OnExitClicked += OnViewExitClicked;

            View.OnShowEntered += OnViewShowEntered;
            View.OnHideEntered += OnViewHideEntered;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            toggleMenuListener.OnPerformed -= OnToggleMenuPerformed;

            View.OnResumeClicked -= OnViewResumeClicked;
            View.OnExitClicked -= OnViewExitClicked;

            View.OnShowEntered -= OnViewShowEntered;
            View.OnHideEntered -= OnViewHideEntered;
        }

        private void OnToggleMenuPerformed(bool value)
        {
            if (View.State is ViewVisibilityState.Hiding or ViewVisibilityState.Hidden)
            {
                View.Show();
                return;
            }

            View.Hide();
        }

        private void OnViewResumeClicked()
        {
            View.Hide();
        }

        private void OnViewExitClicked()
        {
            sceneSystem.LoadMenuScene();
        }

        private void OnViewShowEntered()
        {
            pauseSystem.PauseGame();
            cursorSystem.UnLockCursor();
        }

        private void OnViewHideEntered()
        {
            pauseSystem.ResumeGame();
            cursorSystem.LockCursor();
        }
    }
}
