using UnityEngine;
using Utils.CustomAttributes.ReadOnly;

public class AI : MonoBehaviour
{
    [ReadOnly][SerializeField]
    private int x;
    [ReadOnly][SerializeField]
    private int y;
    [ReadOnly][SerializeField]
    private int turretTypeInt;
}
