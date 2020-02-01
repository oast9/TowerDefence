using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    public static int LivesPlayer;
    public static int LivesEnemy;
    public int startLivesPlayer = 20;
    public int startLivesEnemy = 20;

    public static int Waves;

    public static int role; //Атакующий (1) или защитник (2)

    public static int player;

    void Start()
    {
        Money = startMoney; //Задается, что бы при рестарте игры, деньги начислялись стандартно
        LivesPlayer = startLivesPlayer;
        LivesEnemy = startLivesEnemy;
        Waves = 0;
    }
}
