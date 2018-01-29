using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerWeaponsScript : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject missileObject;

    public float machineGunReload;
    public float missileReload;

    public GameObject currentMissileTarget;
    
    Timer machineGunReloadTimer;
    Timer missileReloadTimer;
    
    void Start()
    {
        machineGunReloadTimer = gameObject.AddComponent<Timer>();
        machineGunReloadTimer.Start();
        missileReloadTimer = gameObject.AddComponent<Timer>();
        missileReloadTimer.Start();
    }

    void Update()
    {
        float inputWeapons = Input.GetAxis("Triggers_1");

        bool inputMachineGun = inputWeapons > 0.1f;
        bool inputMissile = inputWeapons < -0.1f;
            
        if (inputMachineGun && machineGunReloadTimer.GetTime() > machineGunReload)
        {
            GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().direction = transform.forward;
            
            machineGunReloadTimer.Reset();
        }
        
        if (inputMissile && missileReloadTimer.GetTime() > missileReload && currentMissileTarget != null)
        {
            GameObject missile = Instantiate(missileObject, transform.position, Quaternion.identity);
            missile.GetComponent<MissileScript>().target = currentMissileTarget;
            
            missileReloadTimer.Reset();
        }
    }
}