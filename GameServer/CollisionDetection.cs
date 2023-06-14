using System.Diagnostics;
using System.Numerics;

namespace GameServer;

public class CollisionDetection
{
    //Check for collisions with the help of the Collider Data
    private List<StaticCollider> colliders;

    public CollisionDetection()
    {
        //Divide world into cells
        //Detect collision into 
        colliders = LevelDataManager.GetLevelData().colliders;
    }
    
    private bool AABB(Vector3 _position, StaticCollider _collider)
    {
        if (_position.X < _collider.x + _collider.width + .5f &&
            _position.X + .5 > _collider.x &&
            _position.Z < _collider.y + _collider.height + .5f &&
            _position.Z + .5 > _collider.y)
        {
            return true;
        }
        return false;
    }
    
    public bool DetectCollision(Vector3 _position)
    {
        foreach (StaticCollider collider in colliders)
        {
            
            if (AABB(_position, collider))
            {
                Console.WriteLine("Detected Collision!");
                Console.WriteLine($"{collider.x} {collider.y} + {collider.width} + {collider.height}");
                Console.WriteLine($"Player: {_position}");

                Vector3 newPosition = new Vector3(collider.x - _position.X, 0, collider.y - _position.Z);
                Console.WriteLine(newPosition);

                return true;
            }
        }
        return false;
    }
}