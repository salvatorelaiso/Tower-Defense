using System.Collections.Generic;
using Utils;

public class EnemySensorDataList : Singleton<EnemySensorDataList>
{
    private readonly List<EnemySensorData> list = new List<EnemySensorData>();

    public int Count => list.Count;
    
    public List<EnemySensorData> List => list;

    public void Add(EnemySensorData item) =>
        list.Add(item);
    
    public bool Remove(EnemySensorData item) =>
        list.Remove(item);
}