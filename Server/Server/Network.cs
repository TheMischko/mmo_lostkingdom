using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Server.MessageSenders;
using Server.Model.ChatModel;
using ServiceStack;
using Shared.DataClasses;

namespace Server {
    public class Network {
        private const int PORT = 5500;
        private const int MAX_PLAYERS = 100;
        
        public TcpListener serverSocket;
        public static Network instance = new Network();
        public static Client[] clients = new Client[MAX_PLAYERS];

        private Timer tickTimer;

        public void ServerStart() {
            for (int i = 0; i < MAX_PLAYERS; i++) {
                clients[i] = new Client();
            }
            
            serverSocket = new TcpListener(IPAddress.Any, PORT);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
            Console.WriteLine($"Server has started on port {PORT}.");

            tickTimer = new Timer(SendTickUpdates,null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        }

        private void SendTickUpdates(object state) {
            // Main update loop
            foreach (Client client in clients) {
                SendUpdateAsync(client);
            }
        }

        private async Task SendUpdateAsync(Client client) {
            int index = client.index;
            NewChatMessagesSender.SendMessage(index);
        }

        private void OnClientConnect(IAsyncResult result) {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            client.NoDelay = false;
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);

            for (int i = 1; i < MAX_PLAYERS; i++) {
                if (clients[i].socket == null) {
                    clients[i].Initialize(client, i, client.Client.RemoteEndPoint.ToString());
                    Thread.Sleep(100);
                    Console.WriteLine($"Incoming connection from {clients[i].ip} || index: {i}");
                    // Send welcome MSG.
                    UserData[] userData = GetConnectedPlayers().Map(c => c.userData).ToArray();
                    byte[] welcomeMessage = ServerSendData.instance.SendWelcomeMessage(i, userData);
                    SendToClient(i, welcomeMessage);
                    //Broadcast(ServerSendData.instance.SendNewPlayerConnectedMessage(clients[i].userData));
                    return;
                }
            }
        }

        private Client[] GetConnectedPlayers() {
            Client[] connected = Array.FindAll(clients, c => c.socket != null);
            return connected;
        }

        public void SendToClient(int index, byte[] data) {
            TcpClient client = clients[index].socket;
            byte[] dataToSend = new byte[client.SendBufferSize];
            for (int i = 0; i < data.Length; i++) {
                dataToSend[i] = data[i];
            }
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Sending data of size: {dataToSend.Length}");
            stream.Write(dataToSend, 0, dataToSend.Length);
            stream.Flush();
        }
        
        public async Task SendToClientAsync(int index, byte[] data) {
            TcpClient client = clients[index].socket;
            byte[] dataToSend = new byte[client.SendBufferSize];
            for (int i = 0; i < data.Length; i++) {
                dataToSend[i] = data[i];
            }
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Sending data of size: {dataToSend.Length}");
            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
            await stream.FlushAsync();
        }

        public void Broadcast(byte[] data) {
            for (int i = 1; i < MAX_PLAYERS; i++) {
                if (clients[i].socket != null) {
                    SendToClient(i, data);
                }
            }
        }
    }
}