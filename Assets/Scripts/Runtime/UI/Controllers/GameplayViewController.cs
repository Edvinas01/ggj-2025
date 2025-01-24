using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class GameplayViewController : ViewController<GameplayView>
    {
        public void ShowChatText(string text)
        {
            View.ChatText = text;
            View.Show();
        }

        public void HideChatText()
        {
            View.Hide();
        }
    }
}
