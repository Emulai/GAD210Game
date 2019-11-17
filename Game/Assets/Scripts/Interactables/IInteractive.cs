using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    void Activate(PlayerController activator);

    string Info();
}
