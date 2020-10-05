using System;
using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

public enum TurretType
{
        None = 0,
        StandardTurret, StandardTurretUpgraded,
        MissileLauncher, MissileLauncherUpgraded,
        LaserBeamer, LaserBeamerUpgraded,
}

[RequireComponent(typeof(Node))]
public class NodeSensorData : SensorData
{
    private Node node;

    [ReadOnly] [SerializeField]
    private TurretType turretType;
    private int turretTypeInt;
    
    private new void Awake()
    {
        node = GetComponent<Node>();
        UpdateData();
    }

    private void Start() =>
        NodeSensorDataList.Instance.Add(this);

    private void OnDestroy() =>
        NodeSensorDataList.Instance?.Remove(this);
    
    public void SetTurretType(string turretGameObjectName)
    {
        turretGameObjectName = turretGameObjectName.Split('(')[0].Replace("_", "");
        SetTurretType((TurretType) Enum.Parse(typeof(TurretType), turretGameObjectName));
    }

    public void SetTurretType(TurretType turretType)
    {
        this.turretType = turretType;
        turretTypeInt = (int) this.turretType;
    }
}