using System.Collections.Concurrent;
using Utils;

public class EnemySensorDataDictionary : Singleton<EnemySensorDataDictionary>
{
    private readonly ConcurrentDictionary<int, EnemySensorData> dictionary = new ConcurrentDictionary<int, EnemySensorData>();

    public int Count => dictionary.Count;
    
    public ConcurrentDictionary<int, EnemySensorData> List => dictionary;

    public void Add(EnemySensorData item) =>
        dictionary.TryAdd(dictionary.GetHashCode(), item);
    
    public bool Remove(EnemySensorData item) =>
        dictionary.TryRemove(item.GetHashCode(), out item);
}