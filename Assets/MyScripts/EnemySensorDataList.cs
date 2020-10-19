using System.Collections.Concurrent;
using System.Collections.Generic;
using Utils;

public class EnemySensorDataList : Singleton<EnemySensorDataList>
{
    private readonly List<EnemySensorData> dictionary = new List<EnemySensorData>();

    public int Count => dictionary.Count;
    
    public List<EnemySensorData> List => dictionary;

    public void Add(EnemySensorData item) =>
        dictionary.Add(item);
    
    public bool Remove(EnemySensorData item) =>
        dictionary.Remove(item);
}