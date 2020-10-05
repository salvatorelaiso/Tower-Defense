using System;
using UnityEngine;

[Serializable]
public class TurretBlueprint
{
    public int cost;

    public GameObject prefab;
    public int upgradeCost;

    public GameObject upgradedPrefab;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}