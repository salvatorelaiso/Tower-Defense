﻿using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

[RequireComponent(typeof(Enemy))]
public class EnemySensorData : SensorData
{
    private Enemy enemy;

    [ReadOnly] [SerializeField]
    private int healt;

    private new void Awake()
    {
        enemy = GetComponent<Enemy>();
        UpdateData();
    }

    private void Start() =>
        EnemySensorDataList.Instance.Add(this);
    

    protected override void UpdateData()
    {
        base.UpdateData();
        healt = (int) Mathf.Ceil(enemy.Health);
    }

    private void OnDestroy() =>
        EnemySensorDataList.Instance?.Remove(this);
        
}