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
    public List<StaticCollider> colliders;
    public List<System.Numerics.Vector2> spawnPoints;

    public LevelData(List<StaticCollider> _colliders, List<System.Numerics.Vector2> _spawnPoints)
    {
        colliders = _colliders;
        spawnPoints = _spawnPoints;
    }
}

[Serializable]
public class StaticCollider
{
    public float x;
    public float y;

    public float width;
    public float height;

    public StaticCollider(float _x, float _y, float _width, float _height)
    {
        x = _x;
        y = _y;
        width = _width;
        height = _height;
    }
}
