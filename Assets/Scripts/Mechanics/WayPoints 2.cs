using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public List<Transform> pointsPlayer1 = new List<Transform>();
    public List<Transform> pointsPlayer2 = new List<Transform>();
    public int road; //номер дороги
    void Awake() { //Сбор всех точек на карте, для дальнейшего использования врагами
        //pointsPlayer1 = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            pointsPlayer1.Add(transform.GetChild(i));
            pointsPlayer2.Add(transform.GetChild(transform.childCount - 1 - i));
        }
    }
}
