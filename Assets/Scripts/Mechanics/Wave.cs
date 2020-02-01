using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemyLv1;
    public GameObject enemyLv2;
    public GameObject enemyLv3;
    public int count; //количество врагов
    public float rate;  //пауза между появлением врагов
}
