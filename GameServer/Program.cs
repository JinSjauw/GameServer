﻿// See https://aka.ms/new-console-template for more information
using System;
using System.Threading;
using GameServer;

class Program
{
    private static bool isRunning = false;
    
    static void Main(string[] args)
    {
        Console.Title = "Game Server";
        isRunning = true;

        Thread mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();
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
                GameLogic.Update();
                
                nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                if (nextLoop > DateTime.Now)
                {
                    Thread.Sleep(nextLoop - DateTime.Now);
                }
            }
        }
    }
}

