using System;
using System.Numerics;

namespace GameServer;

public struct InputPayload
{
    public uint tick;
    public bool[] inputs;
    public Quaternion rotation;
    
    public override string ToString()
    {
        return $"Tick: {tick}  Inputs: {inputs}";
    }
}
[Serializable]
public struct StatePayload
{
    public uint tick;
    public Vector3 position;
    public Quaternion rotation;

    public override string ToString()
    {
        return $"Tick: {tick}  Position: {position}";
    }
}

public class Player
{
    public int id;
    public string username;
    public uint tick;
    public float timeSent;
    
    public Vector3 position;
    public Quaternion rotation;

    private Queue<InputPayload> inputBuffer;
    private float moveSpeed = 3f / Constants.MS_PER_TICK;
    //private bool[] inputs;

    public Player(int _id, string _username, Vector3 _spawnPosition)
    {
        id = _id;
        username = _username;
        position = _spawnPosition;
        rotation =  Quaternion.Identity;
        inputBuffer = new Queue<InputPayload>();
    }

    public void Update()
    {
         HandleTick();
    }

    private void HandleTick()
    {
        while ( inputBuffer.Count > 0)
        {
            InputPayload _inputPayload = inputBuffer.Dequeue();
            tick = _inputPayload.tick;
            Move(GetInput(_inputPayload.inputs), _inputPayload.rotation);
            ServerSend.PlayerPosition(this);
        }
    }

    private Vector2 GetInput(bool[] _inputs)
    {
        Vector2 _inputDirection = Vector2.Zero;
         
        if (_inputs[0])
        {
            _inputDirection.Y += 1;
        }
        if (_inputs[1])
        {
            _inputDirection.Y -= 1;
        }
        if (_inputs[2])
        {
            _inputDirection.X += 1;
        }
        if (_inputs[3])
        {
            _inputDirection.X -= 1;
        }
        
        return _inputDirection;
    }
    
    private void Move(Vector2 _inputDirection, Quaternion _rotation)
    {
        rotation = _rotation;
        Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
        Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

        Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;
        position += _moveDirection * moveSpeed;
    }

    public void SetClientData(uint _clientTick, float _timeSent, bool[] _inputs, Quaternion _rotation)
    {
        inputBuffer.Enqueue(new InputPayload()
        {
            tick = _clientTick,
            inputs = _inputs,
            rotation = _rotation
        });
        timeSent = _timeSent;
    }
}