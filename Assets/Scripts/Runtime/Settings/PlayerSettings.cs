using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Actors
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(PlayerSettings),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Player Settings",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class PlayerSettings : ScriptableObject
    {
        [Header("Camera")]
        [SerializeField]
        [Min(0f)]
        private float zoomInSpeed = 8f;

        [SerializeField]
        [Min(0f)]
        private float zoomInFov = 20f;

        public float ZoomInSpeed => zoomInSpeed;

        public float ZoomInFov => zoomInFov;
    }
}
