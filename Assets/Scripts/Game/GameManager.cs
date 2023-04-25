using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour {
        public GameObject playerPrefab;
        private List<GameObject> players;

        public void SpawnPlayer() {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
            players.Add(player);
        }

        private void Awake() {
            players = new List<GameObject>();
        }
    }
}