using CHARK.GameManagement;
using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class MoneyViewController : ViewController<MoneyView>
    {
        protected override void Start()
        {
            base.Start();

            View.DisplayText = "0.00 LT";
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            GameManager.AddListener<PlayerCentsChanged>(OnPlayerCentsChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameManager.RemoveListener<PlayerCentsChanged>(OnPlayerCentsChanged);
        }

        private void OnPlayerCentsChanged(PlayerCentsChanged message)
        {
            View.DisplayText = (message.Player.Cents / 100m).ToString("0.00") + " LT";
        }
    }
}
