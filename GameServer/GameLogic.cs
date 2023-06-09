namespace GameServer;

public class GameLogic
{
    public static void Update()
    {
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                _client.player.Update();
            }
        }
        //Console.WriteLine($"Current Tick: {Server.currentTick}");
        ThreadManager.UpdateMain();
    }
}