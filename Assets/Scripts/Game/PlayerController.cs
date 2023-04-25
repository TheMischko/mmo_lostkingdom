using System;
using UnityEngine;

namespace Game {
    public class PlayerController:MonoBehaviour {
        public float movementSpeed = 1f;
        
        private Vector2 movementDir = Vector2.zero;
        private Rigidbody2D rigidbody;

        public void SetDirection(Vector2 direction) {
            movementDir = direction;
        }

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            rigidbody.velocity = movementDir * movementSpeed;
        }
    }
}