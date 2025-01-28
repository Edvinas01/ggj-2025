using CHARK.GameManagement;
using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Systems.Scenes;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class GameOverViewController : ViewController<GameOverView>
    {
        private ISceneSystem sceneSystem;

        protected override void Awake()
        {
            base.Awake();

            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            View.OnRestartGameClicked += OnViewRestartGameClicked;
            View.OnExitGameClicked += OnViewExitGameClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            View.OnRestartGameClicked -= OnViewRestartGameClicked;
            View.OnExitGameClicked -= OnViewExitGameClicked;
        }

        private void OnViewRestartGameClicked()
        {
            sceneSystem.LoadGameplayScene();
        }

        private void OnViewExitGameClicked()
        {
            sceneSystem.LoadMenuScene();
        }
    }
}
