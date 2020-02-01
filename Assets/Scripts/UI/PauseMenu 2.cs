using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    public string menuToLoad = "MainMenu";
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            Toggle();
        }
        
    }

    public void Toggle() {
        ui.SetActive(!ui.activeSelf); //Активация/Деактивация меню паузы

        if (ui.activeSelf) { //если меню паузы активно, останавливаем время
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f; //запускаем время при снятии паузы
        }
    }

    public void Retry() {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu() {
        SceneManager.LoadScene(menuToLoad);
    }
}
