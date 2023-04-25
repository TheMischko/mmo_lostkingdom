using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ServiceStack.DataAnnotations;

namespace Server {
    public class GameServer {
        public static GameServer instance = new GameServer();
        
        public const float tickDelay = 50;
        public EventHandler<ulong> TickHappened;

        private Timer tickTimer;
        private ulong tickCount;
        private List<QueueMessage> messageQueue;

        private GameServer() {
            messageQueue = new List<QueueMessage>();

            tickCount = 0;
            
            tickTimer = new Timer(tickDelay);
            tickTimer.AutoReset = true;
            tickTimer.Elapsed += OnTickTimerElapsed;
        }

        public void Start() {
            tickTimer.Start();
        }

        public ulong GetLatestTick() {
            return tickCount;
        }

        public void AddMessage(int index, byte[] data) {
            byte[] currentTickBytes = BitConverter.GetBytes(this.tickCount);
            byte[] dataWithTick = currentTickBytes.Concat(data).ToArray();
            messageQueue.Add(new QueueMessage(index, dataWithTick));
        }

        public void BroadcastMessage(byte[] data, int[] skipIndices) {
            List<int> indicesToSkip = skipIndices == null ? new List<int>() : new List<int>(skipIndices);
            for (int i = 1; i < Network.MAX_PLAYERS; i++) {
                if (Network.clients[i].socket != null) {
                    if(indicesToSkip.Contains(Network.clients[i].index)) continue;
                    AddMessage(Network.clients[i].index, data);
                }
            }
        }

        private async void OnTickTimerElapsed(object sender, ElapsedEventArgs e) {
            tickCount++;
            TickHappened?.Invoke(this, tickCount);
            
            foreach (QueueMessage message in messageQueue) {
                await Network.instance.SendToClientAsync(message.userIndex, message.data);
            }
            messageQueue.Clear();
        }
        
        private struct QueueMessage {
            public byte[] data;
            public int userIndex;

            public QueueMessage(int index, byte[] data) {
                this.data = data;
                this.userIndex = index;
            }
        }
    }
}