using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionControllerCells : MonoBehaviour
{
    //TODO - изменить алгоритм постройки
    //СНАЧАЛА - выбирается ячейка куда строить/улучшать
    //ЗАТЕМ - выбирается башня/улучшение

    public Color hoverColor; //Цвет подсветки ячейки в тактическом режиме
    public Color notEnoughtMoneyColor; //Цвет подсветки ячейки в тактическом режиме если не хватает денег
    public Vector3 offsetPosition = new Vector3(0f,0.2f,0f); //корректировка установки башни
    public float quadronHoverColor = 0.2f; //корректировка яркости при наводе на квадрат
                                            //в стратегическом режиме
    [HideInInspector]
    public GameObject turret; //туррель в ячейке
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public int upgradeLv = 1;
    public int ownedBy = 0;

    public Renderer rend; //цвет ячейки
    private Color startColor; //изначальный цвет

    private PlayerStats playerStats;
    
    BuildManager buildManager;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerStats>();
        rend = GetComponent<Renderer>();
        startColor = rend.material.color; //сохранение базового цвета
        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition () {
        return transform.position + offsetPosition;
    }

    void OnMouseDown () { //события при нажатии на мышь
        if (EventSystem.current.IsPointerOverGameObject()) //Проверка, что бы не взаимодействовать сквозь UI
            return;
        
        if (gameObject.tag == "Road") //с дорогой нельзя взаимодействовать
            return;

        if (PlayerStats.role != 2) //TODO: ориентировано на одного игрока. Проверяется только один игрок на способность строить
            return;

        if (turret != null) { //Выбор туррели, если она есть в ячейке
            if(turret.GetComponent<TurretParameters>() != null)
                buildManager.SelectCell(this, 1);
            else if(turret.GetComponent<EngineerTower>() != null)
                buildManager.SelectCell(this, 2);
            return;
        }

        if (!buildManager.CanBuild) //Если не можем строить, ничего не делаем
            return;

        //Создание башни, которая выбрана
        BuildTower(buildManager.GetTurretToBuild());
    }

    
    void BuildTower(TurretBlueprint blueprint, int playerControl = 1) {
         if (PlayerStats.Money < blueprint.cost) { //Если не хватает средств - не строим
            Debug.Log("Not enough shards to build that."); //Вывести на экран сообщение
            return;
        }

        GameObject _turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);

        if (ownedBy != playerControl && _turret.GetComponent<EngineerTower>() == null) {
            Debug.Log("На нейтральной территории нельзя строить боевые башни!"); //Вывести на экран сообщение
            Destroy(_turret);
            return;
        }

        PlayerStats.Money -= blueprint.cost; //снятие средств за постройку башни

        turret = _turret;
        if (turret.GetComponent<TurretFindTarget>() != null) {
            turret.GetComponent<TurretFindTarget>().playerControl = playerControl;
            turret.GetComponent<TurretParameters>().playerControl = playerControl;
            offsetPosition = new Vector3(0f,0.2f,0f);
            turret.transform.position = GetBuildPosition();
        }
        
        else if (turret.GetComponent<EngineerTower>() != null) {
            turret.GetComponent<EngineerTower>().playerControl = playerControl;
            offsetPosition = new Vector3(0f,1.5f,0f);
            turret.transform.position = GetBuildPosition();
        }

        turretBlueprint = blueprint;

        GameObject beffect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(beffect, 0.5f); //Сотворить и уничтожить эффект постройки

        Debug.Log("The turret build. Shards left: " + PlayerStats.Money);
    }

    public void ChangeMode(int engineerMode) {
        turret.GetComponent<EngineerTower>().ChangeEngineerMode(engineerMode);
    }

    public void UpgradeTower() {
        if (upgradeLv == 1) {
            if (PlayerStats.Money < turretBlueprint.upgradeCostLv2) { //Если не хватает средств - не улучшаем
                Debug.Log("Not enough shards to upgrade that."); //Вывести на экран сообщение
                return;
            }
        }
        else  if (upgradeLv == 2) {
            if (PlayerStats.Money < turretBlueprint.upgradeCostLv3) {
                Debug.Log("Not enough shards to upgrade that.");
                return;
            }
        }
        
        
        if (turret.GetComponent<TurretFindTarget>() != null) {
            offsetPosition = new Vector3(0f,0.2f,0f);
        }
        
        else if (turret.GetComponent<EngineerTower>() != null) {
            offsetPosition = new Vector3(0f,1.5f,0f);
        }

        GameObject _turret = new GameObject();
        if (upgradeLv == 1) {
            PlayerStats.Money -= turretBlueprint.upgradeCostLv2; //снятие средств за улучшение башни
            //Постройка улучшенной
            _turret = Instantiate(turretBlueprint.upgradedPrefabLv2, GetBuildPosition(), Quaternion.identity);
        }
        else if (upgradeLv == 2) {
            PlayerStats.Money -= turretBlueprint.upgradeCostLv3;
            _turret = Instantiate(turretBlueprint.upgradedPrefabLv3, GetBuildPosition(), Quaternion.identity);
        }
        //Уничтожение старой туррели
        Destroy(turret);

        turret = _turret;

        GameObject beffect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(beffect, 0.5f);

        upgradeLv++;

        Debug.Log("The turret upgraded. Shards left: " + PlayerStats.Money);
    }

    // public void SellTurret() {
    //     if (!isUpgraded)
    //     PlayerStats.Money += turretBlueprint.GetSellAmount();
    //     else
    //     PlayerStats.Money += turretBlueprint.GetUpgradeSellAmount();

    //     //TODO add cool effect

    //     Destroy(turret);
    //     turretBlueprint = null;
    //     isUpgraded = false;
    // }

    void OnMouseEnter() { //подствека при наводке ячейки
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (gameObject.tag == "Road") //с дорогой нельзя взаимодействовать
            return;

        if (!buildManager.CanBuild) //Если не можем строить, ничего не делаем
            return;

        if (buildManager.HasMoney)
            rend.material.color = hoverColor;
        else
            rend.material.color = notEnoughtMoneyColor;
    }


    void OnMouseExit() { //возвращение стандартного цвета при выводе мыши с ячейки
        StandartHoverColor();
    }
    public void QuadreHoverColor() { //подсветка квадрата ячеек в стратегическом режиме
        rend.material.color = new Color(rend.material.color.r + quadronHoverColor,
                                        rend.material.color.g + quadronHoverColor,
                                        rend.material.color.b + quadronHoverColor,
                                        rend.material.color.a);
    }

    public void StandartHoverColor() { //возвращение цвета ячеек
    
            if(ownedBy == 0)
                rend.material.color = startColor;
            if (ownedBy == 1) {
                rend.material.color = Color.green;
            }
            if (ownedBy == 2) {
                rend.material.color = Color.red;
            }
    }
}
