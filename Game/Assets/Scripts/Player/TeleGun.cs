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
        projectileStartScale = projectile.transform.localScale;
        ReturnProjectile();
    }

    public void Shoot()
    {
        if (hasProjectile) 
        {
            Rigidbody rb = projectile.gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.angularDrag = 0.0f;
            rb.AddForce(projectile.transform.forward * 1000.0f);
            projectile.SetParent(null);

            hasProjectile = false;
        }
        else
        {
            ReturnProjectile();    
        }
    }

    public bool GetTeleportTarget(out Vector3 targetPosition)
    {
        targetPosition = projectile.position;
        targetPosition.y += 0.7f;

        if (!hasProjectile)
        {
            ReturnProjectile();
            return true;
        }
        return false;
    }

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

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
