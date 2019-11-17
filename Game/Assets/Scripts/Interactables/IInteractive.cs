using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    // Common interface for interactive objects
    void Activate(PlayerController activator);
    
    // Common interface for interactive info
    string Info();
}
