using System;
using UnityEngine;

namespace Game {
    public class PlayerController:MonoBehaviour {
        public float movementSpeed = 1f;
        public bool isPlayerControlled = false;
        public PlayerState state = PlayerState.Idle;
        
        private Vector2 movementDir = Vector2.zero;
        private Rigidbody2D rigidbody;

        public void SetDirection(Vector2 direction) {
            movementDir = direction;
        }

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            switch (state) {
                case PlayerState.Idle:
                    if (!movementDir.Equals(Vector2.zero)) {
                        state = PlayerState.Moving;
                    }
                    break;
                case PlayerState.Moving:
                    if (movementDir.Equals(Vector2.zero)) {
                        state = PlayerState.Idle;
                    }
                    break;
            }
            rigidbody.velocity = movementDir * movementSpeed;
        }
    }
}