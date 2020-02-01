using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   // public MapGlobalStatus mGS; //глобальный статус карты                                                            
                                                            //для определения позиции камеры в игре
                                                            //возможно введение доп. функций
    public Vector3 standartCameraPos; //стандартное положение камеры
    public Vector3 offsetTransform; //смещение камеры для корректного отображения
    public Vector3 viewTarget;
    public float smoothTime = 0.3f;
    public float panBoardTickness = 50f;
    public GameObject topPanel;
    public GameObject rightPanel;
    public GameObject shop;

    void Start() {
        //mGS = MapGlobalStatus.instance;
        standartCameraPos = transform.position;
        viewTarget = standartCameraPos;
    }

    void Update() {
        if(GameMaster.gameOver) { //Если игра окончена, то управление отключается
            this.enabled = false;
            return;
        }
                //Смещение камеры в сторону целевой позиции если она есть
        if (viewTarget != null) {
            if (Vector3.Distance(transform.position, viewTarget) >= 0.5f){
            Vector3 dir = viewTarget - transform.position;
            dir = dir.normalized;
            transform.Translate(dir * smoothTime * Time.deltaTime, Space.World);
            }        
        }
            //Если курсор переместился к краю экрана - возвращение на экран стратегии
        // if (Input.mousePosition.y >= Screen.height - panBoardTickness ||
        //     Input.mousePosition.x >= Screen.width - panBoardTickness) {
        //         mGS.levelScreenMode = 0;
        //         ReturnToStrategicView();
        //     }
    }
    public void LookAtQuadron(Vector3 pos) { //задание точки для смещения камеры
        viewTarget = new Vector3(pos.x + offsetTransform.x,
                                                    pos.y + offsetTransform.y,
                                                    pos.z + offsetTransform.z);
    }
    public void ReturnToStrategicView() { //задание точки стандартного обзора
        viewTarget = standartCameraPos;
        topPanel.SetActive(false);
        rightPanel.SetActive(false);
        shop.SetActive(false);
    }
}
