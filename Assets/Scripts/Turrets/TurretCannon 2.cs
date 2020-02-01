using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCannon : MonoBehaviour
{
    /*
    TODO
    Добавить видимый радиус пушки
    Сместить табличку улучшения на ячейках с самого верху платформ, т.к. не видно
    Добавить новые улучшения
    Подкрутить баланс
    */
    [Header("Unity Setup Fields")]
    public TurretParameters turretParameters;
    public Transform firePoint; //позиция начала полета пули
    public GameObject bulletPrefab;

    void Update() {
            if (turretParameters.useLaser && turretParameters.target != null) { //Если есть лазер, применяем его вместо стрельбы
                Laser();
            }
            else {
                if (turretParameters.fireCountdown <= 0f && turretParameters.target != null) {
                    Shoot();
                    turretParameters.fireCountdown = 1f / turretParameters.fireRate;
                }
                if (turretParameters.fireCountdown >= 0f)
                    turretParameters.fireCountdown -= Time.deltaTime;
            }
    }

    void Shoot () { //при выстреле создается снаряд из префаба
        GameObject tempBul = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = tempBul.GetComponent<Bullet>();
        
        if(bullet != null) //задание цели снаряду
            bullet.Seek(turretParameters.target);
    }

    void Laser() {
        turretParameters.targetEnemy.TakeDamage(turretParameters.damageOverTime * Time.deltaTime);
         turretParameters.targetEnemy.Slow(turretParameters.slowPrc);
        if(!turretParameters.lineRenderer.enabled)
            turretParameters.lineRenderer.enabled = true;

        turretParameters.lineRenderer.SetPosition(0, firePoint.position);
        turretParameters.lineRenderer.SetPosition(1, turretParameters.target.position);
    }
}
