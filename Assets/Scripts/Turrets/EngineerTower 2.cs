using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerTower : MonoBehaviour
{
    [Header("General")]
    public float health = 15;
    public int playerControl;
    private BoxCollider boxCollider;
    [Header("Engineers")]
    public float engineersMaximumCount;
    public float engineersCount;
    public float engineersSpawnTime;
    public GameObject engineerPrefab;

    
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TurretBase") {
            other.GetComponent<InteractionControllerCells>().ownedBy = playerControl;
            if (playerControl == 1) {
                other.GetComponent<InteractionControllerCells>().rend.material.color = Color.green;
            }
            if (playerControl == 2) {
                other.GetComponent<InteractionControllerCells>().rend.material.color = Color.red;
            }
        }   
    }

    void OnTriggerExit(Collider other)
    {
            if (other.tag == "TurretBase") {
            other.GetComponent<InteractionControllerCells>().ownedBy = 0;
            other.GetComponent<InteractionControllerCells>().StandartHoverColor();
        }  
    }
}
