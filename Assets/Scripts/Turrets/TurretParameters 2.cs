using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretParameters : MonoBehaviour
{   
    [Header("General")]
    public float range = 2f; //Дальность обзора башни
    public float health = 15;
    public Transform target; //Определение цели
    public Enemy targetEnemy;

    [Header("Use Bullets (default)")]
    public float fireRate = 1f; //скорость стрельбы
    public float fireCountdown = 0f; //откат после стрельбы (равно скорости стрельбы)

    [Header("Use Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public float damageOverTime = 1; //урон со временем
    public float slowPrc = .3f; //замедление в процентах
    public Image healthBar;
    public float startHealth;
    public int shards = 10; //количество денег получаемых с башни
    public GameObject turretDieEffect;
    private float dieEffectLive = 0.5f; //продолжительность существования эффекта смерти башни
    public int playerControl;

    
    void Start() {
        
        //playerStats = GameObject.FindGameObjectWithTag($"Player{playerControl}").GetComponent<PlayerStats>();
        health = startHealth;
    }
    
    public void TakeDamage(float damage) {
        health -= damage;

        healthBar.fillAmount = health / startHealth;
        if (health <= 0) {
            Die();
        }
    }

    public void Repair(float repairAmount) {
        health += repairAmount;

        healthBar.fillAmount = health / startHealth;
        if (health > startHealth) {
            health = startHealth;
        }
    }

    void Die() {

        PlayerStats.Money += shards; //получение денег
        WaveSpawner.enemiesAlive--;
        GameObject dieEffect = Instantiate(turretDieEffect, transform.position, transform.rotation); //активация эффекта смерти
        Destroy(gameObject); //уничтожение объекта
        Destroy(dieEffect, dieEffectLive); //уничтожение эффекта смерти
    }
    
}
