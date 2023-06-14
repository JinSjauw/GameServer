using System.Numerics;

namespace GameServer;

public class Projectile
{
    public int projectileID;
    public int clientID;
    public Vector3 position;
    public Vector3 direction;
    public float velocity;
    
    private bool hasHit = false;
    private CollisionDetection collisionDetection = new CollisionDetection();
    
    public Projectile(int _clientId, Vector3 _position, Vector3 _direction, float _velocity)
    {
        clientID = _clientId;
        position = _position;
        direction = _direction;
        velocity = _velocity;
    }
    
    //Process path
    //Check for collisions
    //Update for collisions
    public bool Update()
    {
        
        
        
        //ServerSend.UpdateProjectile(this);
        //Send updated projectile Position
        //If collided send different packet;
        return hasHit;
    }
}