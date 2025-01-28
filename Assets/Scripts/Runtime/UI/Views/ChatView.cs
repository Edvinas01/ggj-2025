using System;
using CHARK.SimpleUI;
using TMPEffects.Components;
using TMPro;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.UI.Views
{
    internal sealed class ChatView : View
    {
        [Header("Text")]
        [SerializeField]
        private TMP_Text chatText;

        [SerializeField]
        private TMPWriter tmpWriter;

        public event Action OnSpeechEntered;

        public event Action OnSpeechExited;

        public string ChatText
        {
            set => chatText.text = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            // TODO

            tmpWriter.OnStartWriter.AddListener(OnStartWriter);
            tmpWriter.OnStopWriter.AddListener(OnStopWriter);
            tmpWriter.OnFinishWriter.AddListener(OnStopWriter);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            tmpWriter.OnStartWriter.RemoveListener(OnStartWriter);
            tmpWriter.OnStopWriter.RemoveListener(OnStopWriter);
            tmpWriter.OnFinishWriter.RemoveListener(OnStopWriter);
        }

        protected override void OnViewHideExited()
        {
            base.OnViewHideExited();

            OnSpeechExited?.Invoke();
        }

        private void OnStartWriter(TMPWriter writer)
        {
            OnSpeechEntered?.Invoke();
        }

        private void OnStopWriter(TMPWriter writer)
        {
            OnSpeechExited?.Invoke();
        }
    }
}
