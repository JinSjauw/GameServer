using System;
using System.Numerics;

namespace GameServer;

public class Player
{
    public int id;
    public string username;
    public uint tick;
    
    public Vector3 position;
    public Quaternion rotation;

    
    private float moveSpeed = 5f / Constants.MS_PER_TICK;
    private bool[] inputs;

    public Player(int _id, string _username, Vector3 _spawnPosition)
    {
        id = _id;
        username = _username;
        position = _spawnPosition;
        rotation =  Quaternion.Identity;

        inputs = new bool[4];
    }

    public void Update()
    {
         Vector2 _inputDirection = Vector2.Zero;
         
         if (inputs[0])
         {
             _inputDirection.Y += 1;
         }
         if (inputs[1])
         {
             _inputDirection.Y -= 1;
         }
         if (inputs[2])
         {
             _inputDirection.X += 1;
         }
         if (inputs[3])
         {
             _inputDirection.X -= 1;
         }
         //Console.WriteLine(_inputDirection + "In Update()");
         Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        //Console.WriteLine(_inputDirection + "In Move()");

        Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
        Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

        Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;
        position += _moveDirection * moveSpeed;
        
        //Console.WriteLine(_moveDirection);
        ServerSend.PlayerPosition(this);
        //ServerSend.PlayerRotation(this);
    }

    public void SetInput(uint _clientTick, bool[] _inputs, Quaternion _rotation)
    {
        tick = _clientTick;
        inputs = _inputs;
        rotation = _rotation;
    }
}