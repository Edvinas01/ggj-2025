using UABPetelnia.GGJ2025.Runtime.Components.Interaction.Interactors;
using UABPetelnia.GGJ2025.Runtime.Constants;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Settings
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(PlayerSettings),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Player Settings",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class PlayerSettings : ScriptableObject, IRaycastInteractorSettings
    {
        [Header("Camera")]
        [SerializeField]
        [Min(0f)]
        private float zoomInSpeed = 8f;

        [SerializeField]
        [Min(0f)]
        private float zoomInFov = 20f;

        [Header("Interaction")]
        [SerializeField]
        private RaycastInteractorData data;

        public float ZoomInSpeed => zoomInSpeed;

        public float ZoomInFov => zoomInFov;

        public float RaycastDistance => data.RaycastDistance;

        public float RaycastRadius => data.RaycastRadius;

        public LayerMask RaycastLayer => data.RaycastLayer;

        public QueryTriggerInteraction QueryTriggerInteraction => data.QueryTriggerInteraction;

        public Color RaycastColor => data.RaycastColor;
    }
}
