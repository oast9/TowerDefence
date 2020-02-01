using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    //глобальный код для постройки

    void Awake()
    {
        if (instance != null) {
            Debug.LogError("Ошибка: Более одного BuildManager на сцене");
            return;
        }    
        instance = this;
    }
    public GameObject buildEffect;
    private TurretBlueprint turretToBuild;
    private InteractionControllerCells selectedCell;
    public NodeUI nodeUI;

    public bool CanBuild { get { return turretToBuild != null; } }
    //Свойство - если turretToBuild не null, возвращаем результат true
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }
    //Свойство - имеются ли средства на покупку турели

    public void SelectCell(InteractionControllerCells cell) {
        if (selectedCell == cell) {
            DeselectCell();
            return;
        }
        selectedCell = cell;
        turretToBuild = null;

        nodeUI.SetTarget(cell);
    }

    public void DeselectCell() {
        selectedCell = null;
        nodeUI.Hide();
    }
    public void SelectTurretToBuild(TurretBlueprint turret) {
        turretToBuild = turret;
        
        DeselectCell();
    }

    public TurretBlueprint GetTurretToBuild() {
        return turretToBuild;
    }
}
