using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleGun : MonoBehaviour
{
    [SerializeField]
    private Transform projectile = null;
    [SerializeField]
    private Transform projectileSpawn = null;

    private bool hasProjectile = true;
    private Vector3 projectileStartScale;

    void Start()
    {
        // Move projectile to gun at start
        projectileStartScale = projectile.transform.localScale;
        ReturnProjectile();
    }

    public void Shoot()
    {
        // If you have projectile, launch it
        if (hasProjectile) 
        {
            Rigidbody rb = projectile.gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.angularDrag = 0.0f;
            rb.AddForce(projectile.transform.forward * 1000.0f);
            projectile.SetParent(null);

            hasProjectile = false;
        }
        // Else call it back
        else
        {
            ReturnProjectile();    
        }
    }

    // Returns true if has projectile & outs position of projectile, plus a smidge on Y
    public bool GetTeleportTarget(out Vector3 targetPosition)
    {
        targetPosition = projectile.position;
        targetPosition.y += 0.7f;

        // Return projectile if not in gun
        if (!hasProjectile)
        {
            ReturnProjectile();
            return true;
        }
        return false;
    }

    // Move projectile to gun launch spot, parent it
    private void ReturnProjectile() 
    {
        Rigidbody rb = projectile.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        projectile.transform.position = projectileSpawn.transform.position;
        projectile.transform.rotation = projectileSpawn.transform.rotation;
        projectile.transform.localScale = projectileStartScale;
        projectile.SetParent(gameObject.transform);

        hasProjectile = true;
    }

    // Update gun's rotation to match cam rotation
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
