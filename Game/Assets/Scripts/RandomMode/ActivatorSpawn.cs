using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorSpawn : MonoBehaviour
{
    private bool inUse = false;

    // Determines if this spawn can be used during scene generation
    public bool IsInUse {
        get { return inUse; }
        set { inUse = value; }
    }
}
