using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

public abstract class SensorData : MonoBehaviour
{
    [ReadOnly] [SerializeField]
    protected int x;

    [ReadOnly] [SerializeField]
    protected int y;

    protected void Awake() =>
        UpdateData();

    protected void LateUpdate() => 
        UpdateData();

    protected virtual void UpdateData()
    {
        var position = transform.position;
        x = Mathf.RoundToInt(position.x) / +5;
        y = Mathf.RoundToInt(position.z) / -5;
    }
}