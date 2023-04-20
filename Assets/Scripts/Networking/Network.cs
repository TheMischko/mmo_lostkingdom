using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net.Sockets;

namespace Networking {
    public class Network : MonoBehaviour {
        public static Network instance;
        [Header("Network Settings")] public string serverIp = "127.0.0.1";
        public int serverPort = 5500;
        public bool isConnected = false;

        public TcpClient playerSocket;
        public NetworkStream myStream;
        public StreamReader reader;
        public StreamWriter writer;

        private byte[] asyncBuffer;
        public bool shouldHandleData;

        public void SendData(byte[] data) {
            NetworkStream stream = playerSocket.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        private void Awake() {
            instance = this;
            ClientReadData.instance.InitMessages();
        }

        private void Start() {
            ConnectToGameServer();
        }

        private void ConnectToGameServer() {
            if (playerSocket != null) {
                if (playerSocket.Connected || isConnected) {
                    return;
                }

                playerSocket.Close();
                playerSocket = null;
            }

            playerSocket = new TcpClient();
            playerSocket.ReceiveBufferSize = 4096;
            playerSocket.SendBufferSize = 4096;
            playerSocket.NoDelay = false;
            Array.Resize(ref asyncBuffer, 8192);
            playerSocket.BeginConnect(serverIp, serverPort, ConnectCallback, null);
            isConnected = true;
            MenuManager.instance._menu = MenuManager.Menu.Home;
        }

        private void ConnectCallback(IAsyncResult result) {
            if (playerSocket != null) {
                playerSocket.EndConnect(result);
                if (!playerSocket.Connected) {
                    isConnected = false;
                }
            }
            else {
                return;
            }

            Debug.Log("You are now connected.");
            playerSocket.NoDelay = true;
            myStream = playerSocket.GetStream();
            myStream.BeginRead(asyncBuffer, 0, playerSocket.ReceiveBufferSize, OnDataReceived, null);
        }

        private void OnDataReceived(IAsyncResult result) {
            if (playerSocket != null) {
                try {
                    int byteSize = myStream.EndRead(result);
                    if (byteSize > 0) {
                        byte[] bytes = new byte[byteSize];
                        Array.Copy(asyncBuffer, 0, bytes, 0, byteSize);
                        ClientReadData.instance.HandleMessage(bytes);
                    }

                    if (!playerSocket.Connected) {
                        Debug.Log("Disconnected from the server.");
                        playerSocket.Close();
                        return;
                    }

                    myStream.BeginRead(asyncBuffer, 0, playerSocket.ReceiveBufferSize, OnDataReceived, null);
                }
                catch (Exception e) {
                    Debug.Log($"Error reading data: {e}");
                    playerSocket.Close();
                    return;
                }
            }
        }
    }
}
