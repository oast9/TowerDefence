using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Unity Setup Fields")]
    public Transform target;
    public float explodeInstanceTime = 0.5f; //время существования эффекта столкновения
    public GameObject instancePrefab; //префаб эффекта столкновения
    [Header("Attributes")]
    public float speed = 2f; //скорость полета снаряда
    public float splashRadius = 0f; //площадный урон
    public float damage = 1; //количество наносимого урона

    void Update()
    {
        if (target== null) { //если нет цели, снаряд уничтожается
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position; //расстояние до объекта
        float distanceThisFrame = speed * Time.deltaTime; //определение пройденнной дистанции

        if (dir.magnitude <= distanceThisFrame) //если всё расстояние до объекта пройдено
        {                                       //то засчитываем попадание
            HitTarget();
        }

        transform.Translate (dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
        //движение снаряда к цели
    }
    
    public void Seek(Transform _target) { //задание пуле цели
        target = _target;
    }

    void HitTarget() {
        GameObject impactInstance = Instantiate(instancePrefab,
        transform.position, transform.rotation); //активация префаба эффектов
        
        if (splashRadius > 0f) { //Если есть площадный урон, то реализуется взрыв
            Explode();
        }
        else {
            Damage(target);
        }

        Destroy(gameObject); //уничтожение снаряда
        Destroy(impactInstance, explodeInstanceTime); //и его эффектов
        
        Debug.Log("Hit!");
        return;
    }

    void Explode() { //Привзрыве создается сфера.
                     //Если коллайдер врага попадает в момент появления сферы - он задет взрывом и получает стандартный урон
        Collider[] colliders = Physics.OverlapSphere(transform.position, splashRadius);
        foreach(Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy) { //Урон по врагу
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage);
    }

    void OnDrawGizmosSelected() { //создание проекции сферы взрыва в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);    
    }
}
