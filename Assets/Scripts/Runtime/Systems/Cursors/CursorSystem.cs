using CHARK.GameManagement.Systems;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Cursors
{
    internal sealed class CursorSystem : MonoSystem, ICursorSystem
    {
        [SerializeField]
        private Texture2D cursorTexture;

        [SerializeField]
        private Vector2 cursorHotspot = new(47f, 6f);

        public bool IsCursorLocked => Cursor.lockState != CursorLockMode.None;

        public override void OnInitialized()
        {
            base.OnInitialized();

            Cursor.SetCursor(
                cursorTexture,
                cursorHotspot,
                CursorMode.ForceSoftware
            );
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnLockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
