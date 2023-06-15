namespace GameServer;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }
    
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }
    
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);
            _packet.Write(Server.serverTime);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void UDPTest(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.udpTest))
        {
            _packet.Write("UDP TEST!");
            
            SendUDPData(_toClient, _packet);
        }
    }

    public static void TimeRequest(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.timeRequest))
        {
            _packet.Write(Server.serverTime);
            SendUDPData(_toClient, _packet);
        }
    }
    
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.position);
            _packet.Write(_player.rotation);
            //Spawn Point
            
            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.tick);
            _packet.Write(_player.timeSent);
            _packet.Write(Server.serverTime);
            _packet.Write(_player.position);
            _packet.Write(_player.rotation);
            
            //Console.WriteLine($"PacketNumber: {_player.tick} + From: {_player.id}");
            
            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnProjectile(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnProjectile))
        {
            _packet.Write(_projectile.clientID);
            _packet.Write(Server.serverTime);
            _packet.Write(_projectile.projectileID);
            _packet.Write(_projectile.position);
            _packet.Write(_projectile.direction);
            _packet.Write(_projectile.velocity);
            
            SendUDPDataToAll(_packet);
        }
    }

    public static void UpdateProjectile(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateProjectile))
        {
            _packet.Write(_projectile.clientID);
            _packet.Write(Server.serverTime);
            _packet.Write(_projectile.projectileID);
            _packet.Write(_projectile.position);

            SendUDPDataToAll(_packet);
        }
    }
}