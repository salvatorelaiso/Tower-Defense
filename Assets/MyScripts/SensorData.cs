using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

public abstract class SensorData : MonoBehaviour
{
    [ReadOnly] [SerializeField]
    private int x;

    [ReadOnly] [SerializeField]
    private int y;

    public int X => x; 
    public int Y => y;
    
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