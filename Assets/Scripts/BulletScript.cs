using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    
    public float damage;

    public string enemyTag;

    public float lifeTime;

    void Start()
    {
        Invoke("ActivateMeshRenderer", 0.025f);
        Destroy(gameObject, lifeTime);
        
        transform.LookAt(transform.position + direction);
    }

    void Update()
    {
        Vector3 newTranslation = new Vector3(0.0f, 0.0f, speed);

        newTranslation *= Time.deltaTime;
        transform.Translate(newTranslation);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            other.gameObject.SendMessage("GetDamage", damage);
            Destroy(gameObject);
        }
    }

    void ActivateMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
}