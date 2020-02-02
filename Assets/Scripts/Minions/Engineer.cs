using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engineer : MonoBehaviour
{
    [Header("Attributes")]
    public float startSpeed = 10f;
   // public string towerTag = "BattleTower";

    public int engineerMode = 1; //1 - ремонт, 2 - захват
    public float assimilateTime = 5f; //скорость захвата построек
    public int repairPower = 1; //сколько хитов лечит
    public float repairRate = 2f;

    public EngineerTower parentTower;
    
    [SerializeField]private float SurRepairRate = 0f;
    [SerializeField]private float SurAssimilateTime = 0f; //скорость захвата построек
    public float speed;
    public float startHealth;
    [SerializeField]private float health;
    //public int shards = 10;
    [SerializeField]private Transform target; //точка куда двигаться
    [SerializeField]private TurretParameters targetTower;
    
    [Header("Unity Setup Fields")]
    public GameObject enemyDieEffect;
    private float dieEffectLive = 0.5f; //продолжительность существования эффекта смерти врага
    public int playerControl; //Кому принадлежит миньон
    public Image healthBar;
    void Start()
    {
        speed = startSpeed;
        health = startHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (engineerMode == 1) { //Поиск и движение к дружеской поврежденной башне и починка
            if (target != null) {
                if(Vector3.Distance(transform.position, target.position) <= 0.5f)
                {
                    if (SurRepairRate >= repairRate) {
                        Repair();
                        SurRepairRate = 0f;
                    }
                    else {
                        SurRepairRate += Time.deltaTime;
                    }
                }
                else {
                    Vector3 dir = target.position - transform.position;
                    transform.Translate(dir.normalized * speed * Time.deltaTime); 

                }
            }
            else {
                FindFriendlyDamagedTower();
            }
        }
        else if (engineerMode == 2) {//Поиск и движение к вражеской башне и захват
            if (target != null) {
                if(Vector3.Distance(transform.position, target.position) <= 0.5f)
                {
                    if (SurAssimilateTime >= assimilateTime) {
                        Assimilate();
                        assimilateTime = 0f;
                    }
                    else {
                        SurAssimilateTime += Time.deltaTime;
                    }
                }
                else {
                    Vector3 dir = target.position - transform.position;
                    transform.Translate(dir.normalized * speed * Time.deltaTime);

                }
            }
            else {
                FindEnemyTower();
            }
        }
        

        
        speed = startSpeed;
        
    }

    public void Repair() {
        targetTower.Repair(repairPower);
    }

    public void Assimilate() {
        targetTower.playerControl = playerControl;
    }

    public void TakeDamage(float damage) {
        health -= damage;

        //healthBar.fillAmount = health / (startHealth * WaveSpawner.healthMult);
        healthBar.fillAmount = health / startHealth;
        if (health <= 0) {
            Die();
        }
    }

    public void Slow(float slowPct) {
        speed = startSpeed * (1f - slowPct);
    }

    void Die() {
        //PlayerStats.Money += shards; //получение денег
        //WaveSpawner.enemiesAlive--;
        parentTower.engineersCount--;
        GameObject dieEffect = Instantiate(enemyDieEffect, transform.position, transform.rotation); //активация эффекта смерти
        Destroy(gameObject); //уничтожение объекта
        Destroy(dieEffect, dieEffectLive); //уничтожение эффекта смерти
    }

    void FindEnemyTower() { //Поиск ближайшей башни
        GameObject[] towers = GameObject.FindGameObjectsWithTag("BattleTower");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTower = null;

        foreach (GameObject tower in towers) {
            if (tower.GetComponent<TurretParameters>().playerControl != playerControl) {
            float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
                if (distanceToTower < shortestDistance) {
                    shortestDistance = distanceToTower;
                    nearestTower = tower;
                }
            }
        }

        if (nearestTower != null) {
            target = nearestTower.transform;
            targetTower = nearestTower.GetComponent<TurretParameters>();
        }
        else
            target = null;
    }

    void FindFriendlyDamagedTower() {
        List<GameObject> towers = new List<GameObject>();
        towers.AddRange(GameObject.FindGameObjectsWithTag("BattleTower"));
        towers.AddRange(GameObject.FindGameObjectsWithTag("EngineerTower"));
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTower = null;

        foreach (GameObject tower in towers) {
            targetTower = tower.GetComponent<TurretParameters>();
            if (targetTower.playerControl == playerControl && targetTower.health < targetTower.startHealth) {
            float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
                if (distanceToTower < shortestDistance) {
                    shortestDistance = distanceToTower;
                    nearestTower = tower;
                }
            }
        }

        if (nearestTower != null) {
            target = nearestTower.transform;
            targetTower = nearestTower.GetComponent<TurretParameters>();
        }
        else {
            target = null;
            targetTower = null;
        }
    }
}
