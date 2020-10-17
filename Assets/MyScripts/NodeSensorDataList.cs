using System.Collections.Generic;
using Utils;

public class NodeSensorDataList : Singleton<NodeSensorDataList>
{
    private readonly List<NodeSensorData> list = new List<NodeSensorData>();

    public int Count => list.Count;
    
    public List<NodeSensorData> List => list;

    public void Add(NodeSensorData item) =>
        list.Add(item);

    public bool Remove(NodeSensorData item) => 
        list.Remove(item);
}