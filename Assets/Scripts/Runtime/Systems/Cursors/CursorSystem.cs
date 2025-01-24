using CHARK.GameManagement.Systems;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Cursors
{
    internal sealed class CursorSystem : MonoSystem, ICursorSystem
    {
        public bool IsCursorLocked => Cursor.lockState != CursorLockMode.None;

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnLockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
