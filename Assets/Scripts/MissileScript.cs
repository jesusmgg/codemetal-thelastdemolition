using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    public float damage;
    
    public GameObject target;

    public GameObject explosionObject;
    public float lifeTime;

    bool destroyed;
    
    void Start()
    {
        destroyed = false;
        
        Invoke("Explode", lifeTime);
        
        transform.LookAt(target.transform.position);
    }
    
    void Update()
    {
        if (!destroyed)
        {
            if (target != null)
            {
                var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                Vector3 newTranslation = new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
                newTranslation *= Time.deltaTime;
                transform.Translate(newTranslation);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target && !destroyed)
        {
            other.gameObject.SendMessage("GetDamage", damage);
            Explode();
        }
    }

    void Explode()
    {
        destroyed = true;
        
        GetComponent<MeshRenderer>().enabled = false;
        
        Instantiate(explosionObject, transform.position, transform.rotation);
        //Destroy(explosionObject, 3.0f);
        Destroy(gameObject, 3.1f);
    }
}