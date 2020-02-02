using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float startSpeed = 10f; //базовая скорость врага
    
    [HideInInspector]
    public float speed; //текущая скорость врага
    public float startHealth;
    private float health; //кол-во жизней
    public int shards = 10; //количество денег получаемых с врага
    public int power = 1; //сила врага
    public float range = 4;
    [SerializeField]private Transform target; //точка куда двигаться
    [SerializeField]private TurretParameters targetTurret;
    private int wavepointIndex = 0;
    
    [Header("Unity Setup Fields")]
    public GameObject enemyDieEffect;
    //public string turretTag;
    private float dieEffectLive = 0.5f; //продолжительность существования эффекта смерти врага
    public int playerControl; //Кому принадлежит миньон
    public int roadToGo; //по какой дороге идти
    private WayPoints wayPoints;
    private Transform surWaypoint;
    public Image healthBar;
    public bool endPath = false;
    private float surAttackCooldown = 0f;
    public float baseAttackCooldown = 0.5f;
    Vector3 groundDistanse;
    void Start()
    {
        wayPoints = GameObject.FindGameObjectWithTag($"Road{roadToGo}").GetComponent<WayPoints>();
        if (playerControl == 1) {
            target = wayPoints.pointsPlayer1[0]; //Изначально таргетирование на первую точку для игрока 1
            surWaypoint = target;

        }
        if (playerControl == 2) {
            target = wayPoints.pointsPlayer2[0]; //и на последнюю, для игрока 2
            surWaypoint = target;
        }
        speed = startSpeed;
        //health = startHealth * WaveSpawner.healthMult; //стартовые жизни с множителем
        health = startHealth;
    }

    void Update()
    {
        if (!endPath) {
            
            Vector3 dir = target.position - transform.position; //определение расстояния до чекпоинта
            transform.Translate(dir.normalized * speed * Time.deltaTime); //движение в сторону точки
            if (FindTurret()) {
                if(Vector3.Distance(transform.position, target.position) <= 0.1f) {
                    if (surAttackCooldown >= baseAttackCooldown) {
                        targetTurret.TakeDamage(power);
                        surAttackCooldown = 0f;
                    } else {
                        surAttackCooldown += Time.deltaTime;
                    }
                }

            }
            else {
                //если враг достиг контрольной точки, берем следующую
                if(Vector3.Distance(transform.position, target.position) <= 0.1f) {
                    if (playerControl == 1)
                        GetNextWaypoint(wayPoints.pointsPlayer1);
                    if (playerControl == 2)
                        GetNextWaypoint(wayPoints.pointsPlayer2);
                }
            }
            speed = startSpeed;

        }

        if (endPath && surAttackCooldown <= 0) {
            surAttackCooldown = baseAttackCooldown;
            AttackBase();
        }
        else if (endPath && surAttackCooldown > 0) {
            surAttackCooldown -= Time.deltaTime;
        }
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
        PlayerStats.Money += shards; //получение денег
        WaveSpawner.enemiesAlive--;
        GameObject dieEffect = Instantiate(enemyDieEffect, transform.position, transform.rotation); //активация эффекта смерти
        Destroy(gameObject); //уничтожение объекта
        Destroy(dieEffect, dieEffectLive); //уничтожение эффекта смерти
    }

    void GetNextWaypoint(List<Transform> wayPoints) {
                //проверка, если враг достиг последней контрольной точки - уничтожение
        if (wavepointIndex >= wayPoints.Count - 1) {
            endPath = true;
            return;
        }
        wavepointIndex++;
        target = wayPoints[wavepointIndex];
        surWaypoint = target;
    }

    void AttackBase() {
        if (playerControl == 1)
            PlayerStats.LivesEnemy -= power; //при достижении последней точки, снятие жизни игрока и уничтожение врага
        else if (playerControl == 2)
            PlayerStats.LivesPlayer -= power;
        health--;
        if (health <= 0) {
            WaveSpawner.enemiesAlive--;
            GameObject dieEffect = Instantiate(enemyDieEffect, transform.position, transform.rotation); //активация эффекта смерти
            Destroy(gameObject); //уничтожение объекта
            Destroy(dieEffect, dieEffectLive); //уничтожение эффекта смерти
        }
    }

        bool FindTurret() {
        List<GameObject> turrets = new List<GameObject>();
        turrets.AddRange(GameObject.FindGameObjectsWithTag("BattleTower"));
        turrets.AddRange(GameObject.FindGameObjectsWithTag("EngineerTower"));
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTurret = null; 

        foreach (GameObject turret in turrets) { 
            if (turret.GetComponent<TurretParameters>().playerControl != playerControl) {

            //groundDistanse = new Vector3(transform.position.x, turret.transform.position.y, transform.position.z);
            
            float distanceToTurret = Vector3.Distance(transform.position,
            turret.transform.position);
                if (distanceToTurret < shortestDistance) {
                                                       
                    shortestDistance = distanceToTurret;
                    nearestTurret = turret;
                }
            }
        }

        if (nearestTurret != null && shortestDistance <= range)
            {
                target = nearestTurret.transform;
                targetTurret = nearestTurret.GetComponent<TurretParameters>();
                return true;
            }
        else {
            target = surWaypoint;
            targetTurret = null;

        }
        return false;

    }

        private void OnDrawGizmosSelected() { //отрисовка области видимости башни
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
