using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorSpawn : MonoBehaviour
{
    private bool inUse = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsInUse {
        get { return inUse; }
        set { inUse = value; }
    }
}
