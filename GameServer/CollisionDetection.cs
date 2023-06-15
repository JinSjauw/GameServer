using System.Diagnostics;
using System.Numerics;

namespace GameServer;

public class CollisionDetection
{
    //Check for collisions with the help of the Collider Data
    private List<Collider> colliders;

    public CollisionDetection()
    {
        //Divide world into cells
        //Detect collision into 
        colliders = LevelDataManager.GetLevelData().colliders;
    }
    
    private bool AABB(Collider _colliderA, Collider _colliderB)
    {
        if (_colliderA.x < _colliderB.x + _colliderB.width + _colliderA.width / 2 &&
            _colliderA.x + _colliderA.width / 2 > _colliderB.x &&
            _colliderA.y < _colliderB.y + _colliderB.height + _colliderA.height / 2 &&
            _colliderA.y + _colliderA.height / 2 > _colliderB.y)
        {
            return true;
        }
        return false;
    }

    public bool DetectCollision(Collider _colliderA)
    {
        foreach (Collider colliderB in colliders)
        {
            if (colliderB == _colliderA)
            {
                continue;
            }
            if (AABB(_colliderA, colliderB))
            {
                Console.WriteLine("Detected Collision!");
                Console.WriteLine($"{colliderB}");
                Console.WriteLine($"Player: {_colliderA}");
                
                /*Vector3 newPosition = new Vector3(colliderB.x - _colliderA.x, 0, colliderB.y - _colliderA.y);
                Console.WriteLine(newPosition);*/

                return true;
            }
        }
        return false;
    }
}