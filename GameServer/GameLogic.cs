namespace GameServer;

public class GameLogic
{
    private static WorldState[] worldStateHistory = new WorldState[Constants.BUFFER_SIZE];
    
    public static void Update()
    {
        //WorldState currentWorldState = new WorldState(Server.serverTick);
        
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                _client.player.Update();
                //currentWorldState.players.Add(_client.player);
            }
        }

        uint bufferIndex = Server.serverTick % Constants.BUFFER_SIZE;
        //worldStateHistory[bufferIndex] = currentWorldState;

        //If player shoots
        //Check when the player thought he fired within 1000ms - 2000ms?
    }
}