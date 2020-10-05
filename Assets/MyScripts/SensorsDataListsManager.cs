using System.Collections.Generic;
using UnityEngine;

public class SensorsDataListsManager : MonoBehaviour
{
    private List<EnemySensorData> enemies;
    private List<NodeSensorData> nodes;

    private void Awake()
    {
        enemies = EnemySensorDataList.Instance.List;
        nodes = NodeSensorDataList.Instance.List;
    }
    
}
