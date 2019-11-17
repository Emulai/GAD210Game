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

    // Start is called before the first frame update
    void Start()
    {
        ttl = timeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;

        if (ttl <= 0.0f) {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.collider.tag == "Player") {
            col.gameObject.GetComponent<PlayerController>().DamageHealth(damage);
            Destroy(this.gameObject);
        }
    }

    public float TimeToLive {
        get { return ttl; }
        set { ttl = value; }
    }
}
