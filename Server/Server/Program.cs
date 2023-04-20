using System;
using System.Threading;
using Database;

namespace Server {
    class Program {
        private static Thread threadConsole;
        private static bool isConsoleRunning;

        public static void Main(string[] args) {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            Network.instance.ServerStart();
            Database.Database.instance.Connect();
            ServerHandleData.instance.InitMessages();
        }

        private static void ConsoleThread() {
            string line;
            isConsoleRunning = true;

            while (isConsoleRunning) {
                line = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(line)) {
                    isConsoleRunning = false;
                    return;
                }
                else {
                    
                }
            }
        }
    }
}