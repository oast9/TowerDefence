using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static bool gameOver;
    public GameObject GameOverUI;

    void Start() {
        gameOver = false;
    }
    void Update()
    {
        if(gameOver)
            return;
            //TODO: сделать экран победы
        if (PlayerStats.LivesPlayer <= 0 || PlayerStats.LivesEnemy <= 0) {
            EndGame();
        }
    }

    void EndGame() {
        gameOver = true;
        GameOverUI.SetActive(true);
    }
}
