using System;
using UnityEngine;

namespace Game.Movement {
    public class CameraFollow : MonoBehaviour {
        public Transform target;
        public float smoothing;
        public bool isFollowing;

        private void Update() {
            if (isFollowing) {
                Vector2 lerped = Vector2.Lerp((Vector2) transform.position, (Vector2) target.position, smoothing);
                transform.position = new Vector3(lerped.x, lerped.y, transform.position.z);
            }
        }
    }
}