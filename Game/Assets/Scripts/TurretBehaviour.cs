using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject eye = null;
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float viewDistance = 5.0f;
    [SerializeField]
    private float viewAngle = 20.0f;
    [SerializeField]
    private Transform bulletSpawnLeft = null;
    [SerializeField]
    private Transform bulletSpawnRight = null;
    [SerializeField]
    private GameObject bullet = null;

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;

    private bool seenPlayer = false;
    private float shootDelay = 0.05f;
    private float shootTimer = 0.0f;
    private bool gunChoice = false;

    void Start() {
        player = FindObjectOfType<PlayerController>().gameObject;
        shootTimer = shootDelay;
    }

    void Update() {

        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < viewAngle && 
            Vector3.Distance(player.transform.position, transform.position) < viewDistance) 
        {
            transform.LookAt(player.transform.position);
            seenPlayer = true;
            // shootTimer = shootDelay;
        }

        if (seenPlayer) {
            RaycastHit hit;
            Physics.Raycast(eye.transform.position, eye.transform.forward, out hit, viewDistance);
            if (debug) {
                Debug.DrawRay(eye.transform.position, eye.transform.forward * viewDistance, Color.red);
            }

            if (hit.collider != null) {

                if (hit.collider.tag == "Player") {

                    shootTimer -= Time.deltaTime;
                    if (shootTimer <= 0.0f) {
                        // Attack player
                        Attack();

                        shootTimer = shootDelay;
                    }
                }
            }
            else {
                seenPlayer = false;
                shootTimer = shootDelay;
            }
        }
        
    }

    private void Attack() {

        GameObject b = Instantiate(
            bullet, 
            gunChoice ? bulletSpawnLeft.position : bulletSpawnRight.position, 
            bullet.transform.rotation, 
            null
        );
        b.transform.LookAt(player.transform.position);
        b.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        Debug.Log("Spawn");

        b.GetComponent<Rigidbody>().AddForce((
            player.transform.position - 
            (gunChoice ? bulletSpawnLeft.position : bulletSpawnRight.position)
        ));

        gunChoice = !gunChoice;
    }

}
