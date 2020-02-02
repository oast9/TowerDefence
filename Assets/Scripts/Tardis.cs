using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tardis : MonoBehaviour
{
    public List<GameObject> cellsFirstRoad;
    public List<GameObject> cellsSecondRoad;
    public List<GameObject> cellsThirdRoad;
    public List<GameObject> freeCellsFirstRoad;
    public List<GameObject> freeCellsSecondRoad;
    public List<GameObject> freeCellsThirdRoad;
    public List<GameObject> entireCells;
    public Transform CellsList;
    private  GameObject choosedCell;
    public TurretBlueprint blueprintBattleTower;
    public TurretBlueprint blueprintEngineerTower;
    public static int Money;
    [SerializeField]private int startMoney = 400;

    public float pauseBetweenActions = 1f;
    private float surPauseBetweenActions = 0f;
    private int randomAction;
    private int actionsCount = 0;
    private int actionsMax = 3;
    private bool engineersRoleChanges = false;
    private int reactionsCount = 0;
    private int reactionsMax = 3;
    public WaveSpawner playerWaveSpawner;
    public WaveSpawner tardisWaveSpawner;
    void Start()
    {
        Money = startMoney;
        foreach (Transform child in CellsList)
         {
                 entireCells.Add(child.gameObject);
         }
    }

    void Update()
    {
        if (surPauseBetweenActions >= pauseBetweenActions) {
            if (PlayerStats.role == 1 && reactionsCount < reactionsMax) {
                reactionsCount++;
                if (playerWaveSpawner.roadToGo == 1) {
                    randomAction = Random.Range(0,2);
                    if (randomAction == 0) {
                        freeCellsFirstRoad = cellsFirstRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret != null).ToList();
                        if (freeCellsFirstRoad.Count() > 0){
                            choosedCell = freeCellsFirstRoad[Random.Range(0,freeCellsFirstRoad.Count - 1)];
                            choosedCell.GetComponent<InteractionControllerCells>().UpgradeTowerByTardis();
                            freeCellsFirstRoad = new List<GameObject>();
                        }
                    }
                    
                    if (randomAction > 0) {
                        freeCellsFirstRoad = cellsFirstRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret == null).ToList();
                        choosedCell = freeCellsFirstRoad[Random.Range(0,freeCellsFirstRoad.Count - 1)];
                        choosedCell.GetComponent<InteractionControllerCells>().BuildTower(blueprintBattleTower, 2);
                        freeCellsFirstRoad = new List<GameObject>();
                    }

                }
                else if (playerWaveSpawner.roadToGo == 2) {
                    randomAction = Random.Range(0,2);
                    if (randomAction == 0) {
                        freeCellsSecondRoad = cellsSecondRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret != null).ToList();
                        if (freeCellsSecondRoad.Count() > 0){
                            choosedCell = freeCellsSecondRoad[Random.Range(0,freeCellsSecondRoad.Count - 1)];
                            choosedCell.GetComponent<InteractionControllerCells>().UpgradeTowerByTardis();
                        }
                    }
                    
                    if (randomAction > 0) {
                        freeCellsSecondRoad = cellsSecondRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret == null).ToList();
                        choosedCell = freeCellsSecondRoad[Random.Range(0,freeCellsSecondRoad.Count - 1)];
                        choosedCell.GetComponent<InteractionControllerCells>().BuildTower(blueprintBattleTower, 2);
                    }

                }
                else if (playerWaveSpawner.roadToGo == 3) {
                    randomAction = Random.Range(0,2);
                    if (randomAction == 0) {
                        freeCellsThirdRoad = cellsThirdRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret != null).ToList();
                        if (freeCellsThirdRoad.Count() > 0){
                            choosedCell = freeCellsThirdRoad[Random.Range(0,freeCellsThirdRoad.Count - 1)];
                            choosedCell.GetComponent<InteractionControllerCells>().UpgradeTowerByTardis();
                        }
                    }
                    
                    if (randomAction > 0) {
                        freeCellsThirdRoad = cellsThirdRoad.Where(x => x.GetComponent<InteractionControllerCells>().turret == null).ToList();
                        choosedCell = freeCellsThirdRoad[Random.Range(0,freeCellsThirdRoad.Count - 1)];
                        choosedCell.GetComponent<InteractionControllerCells>().BuildTower(blueprintBattleTower, 2);
                    }

                }
            }
            if (PlayerStats.role == 2) {
                reactionsCount = 0;
            }
            if (actionsCount < actionsMax) {
                actionsCount++;
                randomAction = Random.Range(0, 3);
                if (randomAction == 0) {
                    tardisWaveSpawner.SpawnUpgradeForTardis();
                }
                if (randomAction == 1) {
                    freeCellsFirstRoad = entireCells.Where(x =>
                    x.GetComponent<InteractionControllerCells>().turret == null && 
                    x.GetComponent<InteractionControllerCells>().ownedBy == 0).ToList();
                    choosedCell = freeCellsFirstRoad[Random.Range(0,freeCellsFirstRoad.Count - 1)];
                    choosedCell.GetComponent<InteractionControllerCells>().BuildTower(blueprintEngineerTower, 2);
                }
                if (randomAction > 1) {
                    freeCellsFirstRoad = entireCells.Where(x =>
                    x.GetComponent<InteractionControllerCells>() != null).ToList();
                    
                    freeCellsFirstRoad = freeCellsFirstRoad.Where(x =>
                        x.GetComponent<InteractionControllerCells>().turret != null).ToList();
                    if (freeCellsFirstRoad.Count() > 0 && freeCellsFirstRoad != null) {
                        freeCellsFirstRoad = freeCellsFirstRoad.Where(x =>
                        x.GetComponent<InteractionControllerCells>().turret.GetComponent<EngineerTower>() != null && 
                        x.GetComponent<InteractionControllerCells>().ownedBy == 2).ToList();
                        choosedCell = freeCellsFirstRoad[Random.Range(0,freeCellsFirstRoad.Count - 1)];
                        choosedCell.GetComponent<InteractionControllerCells>().UpgradeTowerByTardis();
                        freeCellsFirstRoad = new List<GameObject>();
                    }
                }
            }
            if (!engineersRoleChanges) {
                engineersRoleChanges = true;
                freeCellsFirstRoad = entireCells.Where(x =>
                    x.GetComponent<InteractionControllerCells>() != null).ToList();
                    
                freeCellsFirstRoad = freeCellsFirstRoad.Where(x =>
                    x.GetComponent<InteractionControllerCells>().turret != null).ToList();
                    if (freeCellsFirstRoad.Count() > 0) {
                        freeCellsFirstRoad = freeCellsFirstRoad.Where(x =>
                        x.GetComponent<InteractionControllerCells>().turret.GetComponent<EngineerTower>() != null && 
                        x.GetComponent<InteractionControllerCells>().ownedBy == 2).ToList();
                    }

                foreach(GameObject engTowers in freeCellsFirstRoad) {
                    engTowers.GetComponent<InteractionControllerCells>().turret.GetComponent<EngineerTower>().ChangeEngineerMode(Random.Range(1, 3));
                }
                
                freeCellsFirstRoad = new List<GameObject>();

            }


        } else {
            surPauseBetweenActions = Random.Range(pauseBetweenActions, pauseBetweenActions * 2);
        }
    }
}
