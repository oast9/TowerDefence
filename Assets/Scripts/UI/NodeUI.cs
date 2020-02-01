using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private InteractionControllerCells cell;
    public Text upgradeCost;
    //public Text sellCost;
    public Button upgradeButton;
    public GameObject ui;

    public void SetTarget (InteractionControllerCells _cell) {
        cell = _cell;

        transform.position = cell.GetBuildPosition();

        if(cell.upgradeLv < 3) {
            if (cell.upgradeLv == 1) {
                upgradeCost.text = "$" + cell.turretBlueprint.upgradeCostLv2; //стоимость улучшения
               // sellCost.text = "$" + cell.turretBlueprint.GetSellLv2Amount(); //стоимость продажи
            }
            else if (cell.upgradeLv == 2) {
                upgradeCost.text = "$" + cell.turretBlueprint.upgradeCostLv3;
                //sellCost.text = "$" + cell.turretBlueprint.GetSellLv3Amount(); //стоимость продажи
            }

            upgradeButton.interactable = true;
        }
        else if(cell.upgradeLv == 3)  {
            upgradeCost.text = "DONE"; //отображение, что улучшение зеавершено
            //sellCost.text = "$" + cell.turretBlueprint.GetUpgradeSellAmount(); 
            upgradeButton.interactable = false; //кнопка улучшения невозможна для взаимодействия
        }

         ui.SetActive(true);
    }

    public void Hide() {
        ui.SetActive(false);
    }

    public void Upgrade() { //кнопка улучшения
        cell.UpgradeTower();
        BuildManager.instance.DeselectCell();
    }

    // public void Sell() { //кнопка продажи
    //     cell.SellTurret();
    //     BuildManager.instance.DeselectCell();
    // }
}
