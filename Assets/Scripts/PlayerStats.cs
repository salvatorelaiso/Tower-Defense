using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    
    public static int Lives;

    public static int Rounds;
    public int startLives = 20;
    public int startMoney = 400;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;
    }
}