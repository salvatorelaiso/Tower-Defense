using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

public class AI : MonoBehaviour
{
    [ReadOnly] [SerializeField]
    private int x;
    [ReadOnly][SerializeField]
    private int y;
    [ReadOnly][SerializeField]
    private string turretTypeName = "-1";

    private int previousX = -1;
    private int previousY = -1;
    private string previousTurretTypeName = "-1";

    private Dictionary<Tuple<int, int>, Node> nodes;
    private Shop shop;
    
    private void Start()
    {
        x = y = -1;
        var parent = GameObject.Find("Nodes");
        nodes = new Dictionary<Tuple<int, int>, Node>();
        shop = GameObject.Find("Shop").GetComponent<Shop>();
        foreach (Transform row in parent.transform)
        {
            foreach (Transform child in row)
            {
                NodeSensorData nodeSensorData = child.gameObject.GetComponent<NodeSensorData>();
                Node node = child.gameObject.GetComponent<Node>();
                nodes.Add(new Tuple<int, int>(nodeSensorData.X, nodeSensorData.Y), node);
            }
        }
    }

    private void Update()
    {
        if (!AreValuesUpdated()) return;
        previousX = x;
        previousY = y;
        previousTurretTypeName = turretTypeName;
        var position = new Tuple<int, int>(x, y);
        var node = nodes[position];
        if (node.turret == null)
        {
            switch (turretTypeName)
            {
                case "standardTurret":
                    node.BuildTurret(shop.standardTurret);
                    return;
                case "missileLauncher":
                    node.BuildTurret(shop.missileLauncher);
                    return;
                case "laserBeamer":
                    node.BuildTurret(shop.laserBeamer);
                    return;
                default:
                    throw new ArgumentException(turretTypeName);
            }
        }
        else
        {
            switch (turretTypeName)
            {
                case "standardTurretUpgraded":
                    node.UpgradeTurret();
                    return;
                case "missileLauncherUpgraded":
                    node.UpgradeTurret();
                    return;
                case "laserBeamerUpgraded":
                    node.UpgradeTurret();
                    return;
                default:
                    throw new ArgumentException(turretTypeName);
            }
        }
    }

    private bool AreValuesUpdated() =>
        ((previousX != x) || (previousY != y) || (previousTurretTypeName != turretTypeName));
}