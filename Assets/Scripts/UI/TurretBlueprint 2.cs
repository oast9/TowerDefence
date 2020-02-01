using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TurretBlueprint //Не является обработчиком и не взаимодействует с другими объектами. Убран MonoBehavior
{
    //TODO - добавить вариации улучшений туррели в будущем. И ответвления
    public GameObject prefab;
    public int cost;
    public Text costText;


    public GameObject upgradedPrefabLv2;
    public GameObject upgradedPrefabLv3;
    public int upgradeCostLv2;
    public int upgradeCostLv3;
    public Text upgradeCostText;

    void Start() {
        costText.text = "$" + cost.ToString();
        //upgradeCostText.text = "$" + upgradeCost.ToString();
    }

    public int GetSellAmount() { //количество средств с продажи
        return cost / 2;
    }
    public int GetSellLv2Amount() { //с продажи с 2 улучшением
        return (cost / 2) + (upgradeCostLv2 / 2);
    } 

    public int GetSellLv3Amount() {
        return (cost / 2) + (upgradeCostLv3 / 2);
    } 
}
