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

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        uint clientTick = _packet.ReadUint();
        bool[] _inputs = new bool[_packet.ReadInt()];
        //Console.WriteLine($"Input Length: {_inputs.Length}");
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Quaternion _rotation = _packet.ReadQuaternion();
        
        //Store inputs of the client in a buffer;
        
        Server.clients[_fromClient].player.SetInput(clientTick, _inputs, _rotation);
    }
}