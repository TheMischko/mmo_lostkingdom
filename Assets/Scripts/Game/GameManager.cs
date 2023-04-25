using System;
using System.Collections.Generic;
using Game.Movement;
using Shared.DataClasses;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour {
        public static GameManager instance;
        public GameObject playerPrefab;
        private List<GameObject> players;
        private int selfIndex = -1;

        private PlayerData selfToAdd;
        private List<PlayerData> otherPlayersToAdd = new List<PlayerData>();

        public void AddForeignPlayer(int index, PlayerData playerData) {
            // Handle index
            otherPlayersToAdd.Add(playerData);
            
        }

        public void SpawnSelf(int index, PlayerData playerData) {
            selfToAdd = playerData;
            selfIndex = index;
        }

        private void Awake() {
            instance = this;
            players = new List<GameObject>();
        }

        private void Update() {
            if (selfToAdd != null) {
                GameObject self = SpawnPlayer();
                SetPlayer(self, selfToAdd, true);
                selfToAdd = null;
            }

            if (otherPlayersToAdd.Count > 0) {
                foreach (PlayerData playerData in otherPlayersToAdd) {
                    GameObject player = SpawnPlayer();
                    SetPlayer(player, playerData, false);
                }
                otherPlayersToAdd.Clear();
            }
        }

        private GameObject SpawnPlayer() {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
            players.Add(player);
            return player;
        }

        private void SetPlayer(GameObject player, PlayerData playerData, bool isSelf = false) {
            player.transform.position = new Vector3(playerData.posX, playerData.posY, 0);
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.movementSpeed = playerData.stats.speed;
            playerController.isPlayerControlled = true;

            CameraFollow cameraFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
            cameraFollow.target = player.transform;
            cameraFollow.isFollowing = true;
        }
    }
}