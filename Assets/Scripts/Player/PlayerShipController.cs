using GalacticRogue.Meta;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GalacticRogue.Player
{
    /// <summary>
    /// One-finger, target-position movement. It also supports mouse input for editor testing.
    /// Applies ship-specific speed multiplier.
    /// </summary>
    public sealed class PlayerShipController : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float baseSpeed = 8f;
        [SerializeField, Min(0f)] private float stopDistance = 0.015f;
        [SerializeField, Range(0.5f, 1f)] private float screenEdgePadding = 0.7f;

        private Camera gameplayCamera;
        private bool hasMovementTarget;
        private Vector2 movementTarget;
        private float moveSpeed;

        public void MultiplyMoveSpeed(float multiplier) => moveSpeed *= Mathf.Max(1f, multiplier);
        public bool IsMoving => hasMovementTarget && Vector2.Distance(transform.position, ClampToGameplayBounds(movementTarget)) > stopDistance;

        private void Awake()
        {
            gameplayCamera = Camera.main;
            ConfigureShip(ShipDefinition.Get(PlayerProfile.SelectedShip));
        }

        /// <summary>Apply ship's speed multiplier to base speed.</summary>
        public void ConfigureShip(ShipDefinition ship)
        {
            moveSpeed = baseSpeed * ship.SpeedMultiplier;
        }

        private void Update()
        {
            ReadPointerInput();
            MoveShip();
        }

        private void ReadPointerInput()
        {
            Pointer pointer = Pointer.current;
            if (pointer == null)
            {
                hasMovementTarget = false;
                return;
            }

            hasMovementTarget = pointer.press.isPressed;
            if (hasMovementTarget)
            {
                movementTarget = ScreenToGamePosition(pointer.position.ReadValue());
            }
        }

        private void MoveShip()
        {
            if (!hasMovementTarget)
            {
                return;
            }

            Vector2 currentPosition = transform.position;
            Vector2 clampedTarget = ClampToGameplayBounds(movementTarget);

            if (Vector2.Distance(currentPosition, clampedTarget) <= stopDistance)
            {
                return;
            }

            transform.position = Vector2.MoveTowards(currentPosition, clampedTarget, moveSpeed * Time.deltaTime);
        }

        private Vector2 ScreenToGamePosition(Vector2 screenPosition)
        {
            Vector3 worldPosition = gameplayCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -gameplayCamera.transform.position.z));
            return worldPosition;
        }

        private Vector2 ClampToGameplayBounds(Vector2 target)
        {
            float halfHeight = gameplayCamera.orthographicSize - screenEdgePadding;
            float halfWidth = halfHeight * gameplayCamera.aspect;
            return new Vector2(
                Mathf.Clamp(target.x, -halfWidth, halfWidth),
                Mathf.Clamp(target.y, -halfHeight, halfHeight));
        }
    }
}
