using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour, IInteractive
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
    [SerializeField]
    private float shootDelay = 0.05f;
    [SerializeField]
    private float bulletForce = 1.0f;
    [SerializeField]
    private LayerMask playerMask = (1 << 11);

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;

    private bool seenPlayer = false;
    private float shootTimer = 0.0f;
    private bool gunChoice = false;
    private bool isActive = true;
    private float inactiveTimer = 0.0f;

    void Start() {
        player = FindObjectOfType<PlayerController>().gameObject;
        shootTimer = shootDelay;
    }

    void Update() {

        if (isActive) {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < viewAngle && 
                Vector3.Distance(player.transform.position, transform.position) < viewDistance) 
            {
                transform.LookAt(player.transform.position);
                seenPlayer = true;
            }

            if (seenPlayer) {
                RaycastHit hit;
                Physics.Raycast(eye.transform.position, eye.transform.forward, out hit, viewDistance, playerMask);
                if (debug) {
                    Debug.DrawRay(eye.transform.position, eye.transform.forward * viewDistance, Color.red);
                }

                if (hit.collider != null) {
                    shootTimer -= Time.deltaTime;
                    if (shootTimer <= 0.0f) {
                        // Attack player
                        Attack();

                        shootTimer = shootDelay;
                    }
                }
                else {
                    seenPlayer = false;
                    shootTimer = shootDelay;
                }
            }
        }
        else if (inactiveTimer < 1.5f) {
            inactiveTimer += Time.deltaTime;

            Quaternion rot = transform.rotation;
            rot.x = (15.0f * (inactiveTimer / 1.5f));
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        
    }

    private void Attack() {

        Vector3 gunPosition = gunChoice ? bulletSpawnLeft.position : bulletSpawnRight.position;

        GameObject b = Instantiate(
            bullet,
            gunPosition,
            bullet.transform.rotation,
            null
        );

        b.GetComponent<Rigidbody>().AddForce(((player.transform.position - gunPosition) * bulletForce));

        gunChoice = !gunChoice;
    }

    public void Activate(PlayerController activator) {
        if (!seenPlayer && isActive) {
            isActive = false;
        }
    }

    public string Info() {
        if (seenPlayer || !isActive) {
            return "";
        }
        else {
            return "Turn Off";
        }
    }

    public bool IsActive {
        get { return isActive; }
        set { isActive = value; }
    }

}
