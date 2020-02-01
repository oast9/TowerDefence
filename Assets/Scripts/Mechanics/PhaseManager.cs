using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;
    //int role = 1;

    public Text phaseText;
    //private PlayerStats playerStats;

    void Awake()
    {
        //playerStats = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerStats>();
        PlayerStats.role = 2;
        ChangePhase();
        if (instance != null) {
            Debug.LogError("Ошибка: Более одного PhaseManager на сцене");
            return;
        }    
        instance = this;
    }

    public void ChangePhase() {
        if (PlayerStats.role == 1){
            phaseText.text = "Игрок 2 - Атакует\nИгрок 1 - Защищается";
            PlayerStats.role = 2;
            //role = 2;
        }
        else {
            phaseText.text = "Игрок 1 - Атакует\nИгрок 2 - Защищается";
            PlayerStats.role = 1;
           // role = 1;
        }
    }
}
