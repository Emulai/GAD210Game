﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask = (1 << 8);

    void FixedUpdate()
    {
        // RaycastHit hit;
        // Physics.Raycast(
        //     new Vector3(
        //         transform.position.x, 
        //         transform.position.y + 0.5f, 
        //         transform.position.z), 
        //     -transform.up, 
        //     out hit,
        //     1.0f, 
        //     mask);

        // if (hit.collider != null)
        // {
        //     if (hit.collider.tag == "Platform")
        //     {
        //         Debug.Log("Hit Platform");
        //         Rigidbody rb = GetComponent<Rigidbody>();
        //         Destroy(rb);

        //         transform.SetParent(hit.collider.transform);
        //     }
        // }
    }

    void OnCollisionEnter(Collision other)
    {
        
        if (other.collider.tag == "Platform")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);

            transform.SetParent(other.collider.transform.parent);
        }
        
    }
}
