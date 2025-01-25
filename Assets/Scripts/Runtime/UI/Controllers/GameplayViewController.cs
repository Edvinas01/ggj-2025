using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class GameplayViewController : ViewController<GameplayView>
    {
        public void ShowPurchase(PurchaseRequest purchase)
        {
            View.ChatText = purchase.Text;
            View.Show();
        }
    }
}
