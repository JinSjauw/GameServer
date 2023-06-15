namespace GameServer;

using Newtonsoft.Json.Linq;

public class LevelDataManager
{
    //Read the data
    //Store Data in a dictionary
    private static LevelData levelData;
    
    public static void ReadLevelData(int _levelIndex)
    {
        string fileName = "Level_" + _levelIndex + "_Data";
        string path = Path.Combine(Environment.CurrentDirectory, @"LevelData\", fileName);
        
        JObject jsonObj = JObject.Parse(File.ReadAllText(path));
        LevelData jsonData = (LevelData)jsonObj.ToObject(typeof(LevelData));
        
        Console.WriteLine(jsonData.colliders.Count);
        Console.WriteLine(jsonData.spawnPoints.Count);

        foreach (Collider staticCollider in jsonData.colliders)
        {
            staticCollider.colliderType = ColliderType.enviroment;
        }
        
        levelData = jsonData;
    }

    public static LevelData GetLevelData()
    {
        return levelData;
    }
}

[Serializable]
public class LevelData
{
    public List<Collider> colliders;
    public List<System.Numerics.Vector2> spawnPoints;

    public LevelData(List<Collider> _colliders, List<System.Numerics.Vector2> _spawnPoints)
    {
        colliders = _colliders;
        spawnPoints = _spawnPoints;
    }
}

public enum ColliderType
{
    enviroment = 0,
    projectile = 1,
    player = 2,
}

[Serializable]
public class Collider
{
    public float x;
    public float y;

    public float width;
    public float height;

    public ColliderType colliderType;

    public Collider(float _x, float _y, float _width, float _height, ColliderType _colliderType)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
        colliderType = _colliderType;
    }

    public override string ToString()
    {
        return $"X: {x}  Y: {y} Type: {colliderType}";
    }
}
