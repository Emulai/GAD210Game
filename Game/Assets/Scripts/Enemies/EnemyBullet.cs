using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float timeToLive = 10.0f;
    [SerializeField]
    private float damage = 1.0f;

    private float ttl = 0.0f;

    void Start()
    {
        ttl = timeToLive;
    }

    void Update()
    {
        ttl -= Time.deltaTime;

        if (ttl <= 0.0f) {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.tag == "Player") {
            // If bullet hits player, damage player
            col.gameObject.GetComponent<PlayerController>().DamageHealth(damage);
            Destroy(this.gameObject);
        }
    }

    // Used by SaveSystem
    public float TimeToLive {
        get { return ttl; }
        set { ttl = value; }
    }
}
