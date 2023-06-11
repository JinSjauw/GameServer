using System.Diagnostics;
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
            Server.clients[_fromClient].player.SetClientData(_clientTick, _timeSent, _inputs, _rotation);
        }
    }
}