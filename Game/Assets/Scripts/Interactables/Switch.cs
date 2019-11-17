using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Used to provide common switch interface
    protected bool isActive = false;

    public bool IsActive {
        get { return isActive; }
        set { isActive = value; }
    }
}
