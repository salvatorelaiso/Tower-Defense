using System.Collections.Generic;
using UnityEngine;

public class SensorsDataListsManager : MonoBehaviour
{
    public List<EnemySensorData> enemies;
    public List<NodeSensorData> nodes;

    private void Awake()
    {
        enemies = EnemySensorDataList.Instance.List;
        nodes = NodeSensorDataList.Instance.List;
    }
    
}
