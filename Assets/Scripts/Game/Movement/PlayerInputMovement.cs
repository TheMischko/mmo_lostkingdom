using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Movement {
    public class PlayerInputMovement : BaseMovement {
        [SerializeField] private InputActionReference movementHoriz;
        [SerializeField] private InputActionReference movementVertical;

        private Vector2 direction;
        protected void Awake() {
            base.Awake();
            direction = Vector2.zero;
            movementHoriz.action.performed += OnMovementHoriz;
            movementVertical.action.performed += OnMovementVert;
        }

        private void OnMovementHoriz(InputAction.CallbackContext context) {
            float dir = context.ReadValue<float>();
            direction.x = dir;
            Debug.Log(direction);
            OnChangeDirection(direction);
        }
        private void OnMovementVert(InputAction.CallbackContext context) {
            float dir = context.ReadValue<float>();
            direction.y = dir;
            Debug.Log(direction);
            OnChangeDirection(direction);
        }
    }
}