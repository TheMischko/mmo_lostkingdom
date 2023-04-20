using System;
using System.Net.Sockets;
using Shared.DataClasses;
using Shared.Utils;

namespace Server {
    public class Client {
        public int index;
        public string ip;
        public TcpClient socket;
        public NetworkStream myStream;
        public UserData userData;

        private byte[] readBuffer;

        public void Start() {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            myStream = socket.GetStream();
            Array.Resize(ref readBuffer, socket.ReceiveBufferSize);
            myStream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
        }

        public void Initialize(TcpClient client, int index, string ip) {
            this.socket = client;
            this.index = index;
            this.ip = ip;
            Random rnd = new Random();
            this.userData = new UserData(index, "name" + rnd.Next());
            Start();
        }

        public bool isConnected() {
            return socket != null && socket.Client != null && socket.Client.Connected;
        }

        private void OnReceiveData(IAsyncResult result) {
            try {
                int streamBytesCount = myStream.EndRead(result);
                if (socket == null) {
                    return;
                }

                if (streamBytesCount <= 0) {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, streamBytesCount);
                Buffer.BlockCopy(readBuffer, 0, newBytes, 0, streamBytesCount);
                Console.WriteLine("Got new message.");
                BufferReader bufferReader = new BufferReader(newBytes);
                
                // Handle data
                ServerHandleData.instance.HandleMessage(index, newBytes);
                
                if (socket == null) {
                    return;
                }

                myStream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                CloseConnection();
                return;
            }
        }

        private void CloseConnection() {
            socket.Close();
            socket = null;
            Console.WriteLine($"Connection destroyed from: {ip}");
        }
    }
}