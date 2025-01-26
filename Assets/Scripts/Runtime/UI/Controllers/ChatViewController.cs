using System;
using CHARK.SimpleUI;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UABPetelnia.GGJ2025.Runtime.UI.Views;

namespace UABPetelnia.GGJ2025.Runtime.UI.Controllers
{
    internal sealed class ChatViewController : ViewController<ChatView>
    {
        public event Action OnSpeechEntered;

        public event Action OnSpeechExited;

        protected override void OnEnable()
        {
            base.OnEnable();

            View.OnSpeechEntered += OnViewSpeechEntered;
            View.OnSpeechExited += OnViewSpeechExited;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            View.OnSpeechEntered -= OnViewSpeechEntered;
            View.OnSpeechExited -= OnViewSpeechExited;
        }

        public void ShowPurchase(PurchaseRequest purchase)
        {
            View.ChatText = purchase.Text;
            View.Show();
        }

        private void OnViewSpeechEntered()
        {
            OnSpeechEntered?.Invoke();
        }

        private void OnViewSpeechExited()
        {
            OnSpeechExited?.Invoke();
        }
    }
}
