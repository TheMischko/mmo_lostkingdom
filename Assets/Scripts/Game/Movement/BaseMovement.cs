using System;
using UnityEngine;

namespace Game.Movement {
    public class BaseMovement : MonoBehaviour {
        private PlayerController playerController;

        protected void Awake() {
            playerController = GetComponent<PlayerController>();
        }

        protected void OnChangeDirection(Vector2 direction) {
            playerController.SetDirection(direction.normalized);
        }
    }
}