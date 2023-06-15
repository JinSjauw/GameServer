using System.Numerics;

namespace GameServer;

public enum ProjectileTypes
{
    pistolBullet = 0,
    rifleBullet = 1,
    bolt = 2,
}

public class Projectile
{
    public int projectileID;
    public int clientID;
    public int hitClientID = 0;
    public Vector3 position;
    public Vector3 direction;
    public float velocity;
    public Collider projectileCollider;

    private bool hasHit = false;
    private CollisionDetection collisionDetection;
    
    public Projectile(int _clientID, Vector3 _position, Vector3 _direction, float _velocity)
    {
        clientID = _clientID;
        position = _position;
        direction = _direction;
        direction.Y = 0;
        velocity = _velocity;
        projectileCollider = new Collider(_position.X, _position.Z, .1f, .1f, ColliderType.projectile);
        collisionDetection = new CollisionDetection();
    }

    private Vector3 Move()
    {
        Vector3 newPosition = position + direction * velocity * Constants.MS_PER_SECOND;
        return newPosition;
    }
    
    //Process path
    //Check for collisions
    //Update for collisions
    public bool Update()
    {
        Vector3 newPosition = Move();
        projectileCollider.x = position.X;
        projectileCollider.y = position.Z;
        
        //Console.WriteLine("PlayerColliders: " + Server.playerColliders[0]);
        Collider newCollider = new Collider(newPosition.X, newPosition.Z, .3f, .3f, ColliderType.projectile);
        if (!collisionDetection.DetectCollision(newCollider) && !collisionDetection.DetectCollision(newCollider, Server.playerColliders))
        {
            position = newPosition;
        }
        else
        {
            //Update position on where it would have collided
            Collider hitCollider = collisionDetection.GetLastHit();
            if (hitCollider.colliderType == ColliderType.player)
            {
                hitClientID = hitCollider.playerID;
                Console.WriteLine("Hit player: " + hitClientID);
                //ServerSend DamagePlayer
                Server.clients[hitClientID].player.TakeDamage(clientID, 20);
            }
            ServerSend.UpdateProjectile(this);
            hasHit = true;
        }
        //Send updated projectile Position
        //If collided send different packet;
        return hasHit;
    }
}