// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Threading;
using GameServer;

class Program
{
    private static bool isRunning = false;
    private static GameLogic gameLogic;
    
    static void Main(string[] args)
    {
        Console.Title = "Game Server";
        isRunning = true;
        
        Thread mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();
        gameLogic = new GameLogic();
        LevelDataManager.ReadLevelData(0);
        Server.Start(50, 26950);
        Console.WriteLine("Hello, World!");
    }
    
    private static void MainThread()
    {
        Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second");
        DateTime nextLoop = DateTime.Now;
        while (isRunning)
        {
            while (nextLoop < DateTime.Now)
            {
                ThreadManager.UpdateMain();
                gameLogic.Update();
                Server.serverTick++;
                Server.serverTime += Constants.MS_PER_TICK / 1000f;
                //Console.WriteLine(Server.serverTime);
                nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                if (nextLoop > DateTime.Now)
                {
                    Thread.Sleep(nextLoop - DateTime.Now);
                }
            }
        }
    }
}

