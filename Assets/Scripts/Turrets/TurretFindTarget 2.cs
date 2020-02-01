using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFindTarget : MonoBehaviour
{
 
    
    [Header("Unity Setup Fields")]
    public Transform partToRotate;
    public TurretParameters turretParameters;
    public int playerControl;
    
    public float turnCannonSpeed = 10f; //скорость поворота башни
    public string enemyTag = "Enemy";


    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Повторение функции поиска цели
        //Создано для снижения затрат мощности процессора
    }
    
    void Update()
    {
        if (turretParameters.target == null) //Если нет цели, ничего не делаем
        {
            if (turretParameters.useLaser) {
                if(turretParameters.lineRenderer.enabled)
                    turretParameters.lineRenderer.enabled = false;
            }
            return;
        }
        LockOnTarget();
    }

    void LockOnTarget() { //Закрепление цели и стрельба
        //Цель найдена
        Vector3 dir = turretParameters.target.transform.position - transform.position; //расстояние
        Quaternion lookRotation = Quaternion.LookRotation(dir); //определение куда смотреть
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
                                            lookRotation,
                                            Time.deltaTime * turnCannonSpeed).eulerAngles; //получение углов Эйлера для позиционирования
        partToRotate.rotation = Quaternion.Euler (rotation.x, rotation.y, 0f);
        //Конечное позиционирование пушки относительно цели
    }

    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //занесение всех врагов в массив
        float shortestDistance = Mathf.Infinity; //расстояние до цели изначально бесконечность
        GameObject nearestEnemy = null; //ближайшего врага пока нет

        foreach (GameObject enemy in enemies) { //проходим по всем врагам
            if (enemy.GetComponent<Enemy>().playerControl != playerControl) {
            float distanceToEnemy = Vector3.Distance(transform.position,
            enemy.transform.position);
            //Вычисляем расстояние к врагу относительно башни
                if (distanceToEnemy < shortestDistance) { //если враг ближе, чем предыдущее расстояние,
                                                        //то записываем его как ближайшую цель
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= turretParameters.range) //определение конечной цели, если она есть
            {
            turretParameters.target = nearestEnemy.transform;
            turretParameters.targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }
        else
            turretParameters.target = null;

    }
    
    private void OnDrawGizmosSelected() { //отрисовка области видимости башни
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretParameters.range);
    }
}
