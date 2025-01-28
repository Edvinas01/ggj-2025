using System.Collections.Generic;
using CHARK.GameManagement;
using UABPetelnia.GGJ2025.Runtime.Actors;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Components.Triggers
{
    internal sealed class HealthTrigger : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> hearts;

        private void OnEnable()
        {
            GameManager.AddListener<PlayerHealthChanged>(OnPlayerHealthChanged);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerHealthChanged>(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(PlayerHealthChanged message)
        {
            var health = message.Player.Health;
            for (var index = 0; index < hearts.Count; index++)
            {
                var heart = hearts[index];
                heart.SetActive(index < health);
            }
        }
    }
}
