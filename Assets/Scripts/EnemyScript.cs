using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    public float bulletRange;
    public float missileRange;

    public float bulletReload;
    public float missileReload;
    
    public float hitPoints;
    float currentHitPoints;

    public GameObject bulletObject;
    public GameObject missileObject;

    public GameObject player;
    public GameObject targetUIElement;

    public GameObject explosionObject;

    Timer bulletReloadTimer;
    Timer missileReloadTimer;

    void Start()
    {
        bulletReloadTimer = gameObject.AddComponent<Timer>();
        bulletReloadTimer.Start();
        missileReloadTimer = gameObject.AddComponent<Timer>();
        missileReloadTimer.Start();

        currentHitPoints = hitPoints;
    }

    void Update()
    {
        SeekPlayer();
        Attack();
    }

    void SeekPlayer()
    {
        var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        Vector3 newTranslation = new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        newTranslation *= Time.deltaTime;
        transform.Translate(newTranslation);
    }

    void Attack()
    {
        if (bulletObject != null && bulletReloadTimer.GetTime() > bulletReload)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bulletRange);
            int i = 0;
            
            while (i < hitColliders.Length)
            {
                GameObject target = hitColliders[i].gameObject;
                if (target.CompareTag("Player"))
                {
                    GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
                    bullet.GetComponent<BulletScript>().direction = transform.forward;
            
                    bulletReloadTimer.Reset();
                    break;
                }
                
                i++;
            }
        }
    }

    public void GetDamage(int damage)
    {
        currentHitPoints -= damage;

        if (currentHitPoints <= 0)
        {
            Instantiate(explosionObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (targetUIElement != null) Destroy(targetUIElement);
    }
}