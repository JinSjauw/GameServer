using System.Diagnostics;
using System.Numerics;

namespace GameServer;

public class CollisionDetection
{
    //Check for collisions with the help of the Collider Data
    private List<Collider> colliders;
    private Collider lastHit = null;
    
    public CollisionDetection()
    {
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
                lastHit = colliderB;
                return true;
            }
        }
        return false;
    }
    
    public bool DetectCollision(Collider _colliderA, List<Collider> _colliderList)
    {
        foreach (Collider colliderB in _colliderList)
        {
            if (colliderB == _colliderA)
            {
                continue;
            }
            if (AABB(_colliderA, colliderB))
            {
                /*Console.WriteLine("Detected Collision!");
                Console.WriteLine($"B: {colliderB}");
                Console.WriteLine($"A: {_colliderA}");*/
                lastHit = colliderB;
                return true;
            }
        }
        return false;
    }
    

    public Collider GetLastHit()
    {
        return lastHit;
    }
}