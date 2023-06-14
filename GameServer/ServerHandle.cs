using System.Numerics;
namespace GameServer;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        
        Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Console.WriteLine($"Player \"{_username}\" (ID:{_fromClient}) has assume the wrong client ID ({_clientIdCheck})!");
        }
        // send player into the game
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void UDPTestReceived(int _fromClient, Packet _packet)
    {
        string _msg = _packet.ReadString();
        
        Console.WriteLine($"Received UDP Packet crom client. MESSAGE: {_msg}");
    }

    public static void TimeRequest(int _fromClient, Packet _packet)
    {
        if (Server.clients.ContainsKey(_fromClient))
        {
            ServerSend.TimeRequest(_fromClient);
        }
    }
    
    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        uint _clientTick = _packet.ReadUint();
        float _timeSent = _packet.ReadFloat();
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        if (Server.clients.ContainsKey(_fromClient))
        {
            Server.clients[_fromClient].player.SetPlayerData(_clientTick, _timeSent, _inputs, _rotation);
        }
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        uint _clientTick = _packet.ReadUint();
        float _timeSent = _packet.ReadFloat();
        Vector3 _position = _packet.ReadVector3();
        Vector3 _direction = _packet.ReadVector3();
        float _velocity = _packet.ReadFloat();
            
        //Calculate collision from projectile with angle + position + velocity;
        //Spawn Bullet
        //Communicate to all clients to spawn bullet with(_fromClient)
        //Create new projectile Instance
        //Inject instance to all clients
        Projectile _projectile = new Projectile(_fromClient,_position, _direction, _velocity);
        ProjectileManager.Add(_projectile);
        
        if (Server.clients.ContainsKey(_fromClient))
        {
            Server.clients[_fromClient].player.SpawnProjectile(_projectile);
        }
    }
}