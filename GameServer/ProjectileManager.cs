namespace GameServer;

public class ProjectileManager
{
    private static List<Projectile> projectilesList = new List<Projectile>();
    private static List<Projectile> hitList = new List<Projectile>();

    public static int projectileID = 0;
    
    public static void Add(Projectile _projectile)
    {
        projectilesList.Add(_projectile);
        _projectile.projectileID = projectileID++;
    }

    public static void Update()
    {
        if (projectilesList.Count > 0)
        {
            foreach (Projectile projectile in projectilesList)
            {
                if (projectile.Update())
                {
                    hitList.Add(projectile);
                }
            }
        }

        if (hitList.Count > 0)
        {
            foreach (Projectile projectile in hitList)
            {
                projectilesList.Remove(projectile);
            }
            hitList.Clear();
        }
    }
}