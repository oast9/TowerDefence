using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public Text wavesCount;
    public string menuToLoad = "MainMenu";
    void OnEnable() {
        wavesCount.text = PlayerStats.Waves.ToString();
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Перезапуск сцены
    }

    public void Menu() {
        SceneManager.LoadScene(menuToLoad);
    }
}
