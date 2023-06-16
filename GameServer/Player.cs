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
    public Vector2 inputDirection;

    public Collider playerCollider;
    public CollisionDetection collisionDetection;

    public int kills = 0;
    public int deaths = 0;
    
    private Queue<InputPayload> inputBuffer;
    private float moveSpeed = 5f;
    private float maxHP = 100;
    private float HP;

    public Player(int _id, string _username, Vector3 _spawnPosition)
    {
        id = _id;
        username = _username;
        position = _spawnPosition;
        rotation =  Quaternion.Identity;
        inputBuffer = new Queue<InputPayload>();
        collisionDetection = new CollisionDetection();
        playerCollider = new Collider(_spawnPosition.X, _spawnPosition.Z, 1, 1, ColliderType.player);
        playerCollider.playerID = _id;
        HP = maxHP;
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
            inputDirection = GetInput(_inputPayload.inputs);
            Move(inputDirection, _inputPayload.rotation);
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
        Vector3 _newPosition = position + _moveDirection * moveSpeed * Constants.MS_PER_SECOND;
        //Check for collisions
        Collider newCollider = new Collider(_newPosition.X, _newPosition.Z, 1, 1, ColliderType.player);
        
        if (!collisionDetection.DetectCollision(newCollider))
        {
            position = _newPosition;
            playerCollider.x = _newPosition.X;
            playerCollider.y = _newPosition.Z;
        }
    }

    public void SetPlayerData(uint _clientTick, float _timeSent, bool[] _inputs, Quaternion _rotation)
    {
        inputBuffer.Enqueue(new InputPayload()
        {
            tick = _clientTick,
            inputs = _inputs,
            rotation = _rotation
        });
        timeSent = _timeSent;
    }

    public void TakeDamage(int _clientID, int _damage)
    {
        HP -= _damage;
        ServerSend.PlayerDamage(id, _damage);
        
        if (HP <= 0)
        {
            inputBuffer.Clear();
            ServerSend.PlayerDie(id);
            deaths++;
            Vector2 newSpawnPoint = LevelDataManager.GetRandomSpawnPoint();
            position = new Vector3(newSpawnPoint.X, 0, newSpawnPoint.Y);
            HP = maxHP;
            ServerSend.PlayerRespawn(id, newSpawnPoint);
            ServerSend.PlayerPosition(this);
            
            if (Server.clients.ContainsKey(_clientID))
            {
                Server.clients[_clientID].player.kills++;
                ServerSend.PlayerScoreKill(_clientID);
            }
        }
    }

}