using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerTower : MonoBehaviour
{
    [Header("General")]
    public float health = 15;
    public int playerControl;
    private BoxCollider boxCollider;
    [Header("Engineers")]
    public int engineersMaxCount;
    public int engineersCount;
    public float engineersSpawnTime = 5f;
    private float surEngineersSpawnTime = 0f;
    public float engineerSpawnVector_Y;
    public Vector3 engineerSpawnPlace;
    public GameObject engineerPrefab;

    [SerializeField]private int engineerMode = 1;

    List<GameObject> surEngineers = new List<GameObject>();
    public Image healthBar;
    public float startHealth;
    public int shards = 10; //количество денег получаемых с башни
    public GameObject turretDieEffect;
    private float dieEffectLive = 0.5f; //продолжительность существования эффекта смерти башни
    private float dieTime = 0.1f;

    
    void Start()
    {
        engineerSpawnPlace = new Vector3(transform.position.x, engineerSpawnVector_Y, transform.position.z);
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (surEngineersSpawnTime >= engineersSpawnTime) {
            if (engineersCount < engineersMaxCount) {
                SpawnEngineer();
                surEngineersSpawnTime = 0f;
            }
        }
        else
            surEngineersSpawnTime += Time.deltaTime;
    }

    private void SpawnEngineer() {
        GameObject engineer = Instantiate(engineerPrefab, engineerSpawnPlace, Quaternion.identity);
        engineer.GetComponent<Engineer>().playerControl = playerControl;
        engineer.GetComponent<Engineer>().parentTower = this;
        engineer.GetComponent<Engineer>().engineerMode = engineerMode;
        engineersCount++;
        surEngineers.Add(engineer);
    }

    public void ChangeEngineerMode(int mode) {
        engineerMode = mode;
        foreach(GameObject engineer in surEngineers) {
            if(engineer.GetComponent<Engineer>() != null)
                engineer.GetComponent<Engineer>().engineerMode = engineerMode;
        }
    }

        public void TakeDamage(float damage) {
        health -= damage;

        healthBar.fillAmount = health / startHealth;
        if (health <= 0) {
            Die();
        }
    }

        void Die() {
        foreach(GameObject engineer in surEngineers) {
            if(engineer.GetComponent<Engineer>() != null)
                engineer.GetComponent<Engineer>().TakeDamage(engineer.GetComponent<Engineer>().startHealth);
        }
        PlayerStats.Money += shards; //получение денег
        GameObject dieEffect = Instantiate(turretDieEffect, transform.position, transform.rotation); //активация эффекта смерти
        transform.position = new Vector3(transform.position.x, -100, transform.position.y);
        Destroy(gameObject, dieTime); //уничтожение объекта
        Destroy(dieEffect, dieEffectLive); //уничтожение эффекта смерти
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TurretBase") {
            if (other.GetComponent<InteractionControllerCells>().ownedBy == 0) {
                other.GetComponent<InteractionControllerCells>().ownedBy = playerControl;
                if (playerControl == 1) {
                    other.GetComponent<InteractionControllerCells>().rend.material.color = Color.green;
                }
                if (playerControl == 2) {
                    other.GetComponent<InteractionControllerCells>().rend.material.color = Color.red;
                }
            }
        }   
    }

    void OnTriggerExit(Collider other)
    {
            if (other.tag == "TurretBase" && other.GetComponent<InteractionControllerCells>().ownedBy == playerControl) {
            other.GetComponent<InteractionControllerCells>().ownedBy = 0;
            other.GetComponent<InteractionControllerCells>().StandartHoverColor();
        }  
    }
}
