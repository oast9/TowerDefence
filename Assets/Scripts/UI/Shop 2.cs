using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TurretBlueprint baseTurret;
    public TurretBlueprint engineerTurret;
    // public TurretBlueprint heavyCannon;
    // public TurretBlueprint laserTurret;

    //TODO - Добавить исчезновение панельки в тактическом режиме
    BuildManager buildManager;

    WaveSpawner waveSpawnerPlayer1;

    void Start() {
        buildManager = BuildManager.instance;
        waveSpawnerPlayer1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<WaveSpawner>();
    }
    public void SelectStandartTurret() { //Выбор туррели для покупки
        Debug.Log("Buy Standart Turret");
        buildManager.SelectTurretToBuild(baseTurret);
    }
    
        public void SelecEngineerTurret() {
        Debug.Log("Buy Engineer Turret");
        buildManager.SelectTurretToBuild(engineerTurret);
    }

    public void SpawnUpgradeForPlayer() {
        waveSpawnerPlayer1.SpawnUpgrade();
    }


    // public void SelectHeavyCannonTurret() {
    //     Debug.Log("Buy Heavy Cannon Turret");
    //     buildManager.SelectTurretToBuild(heavyCannon);
    // }

    // public void SelectLaserTurret() {
    //     Debug.Log("Buy Laser Turret");
    //     buildManager.SelectTurretToBuild(laserTurret);
    // }
}
