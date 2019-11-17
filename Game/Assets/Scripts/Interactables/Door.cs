using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DoorSwitch {
    public bool doorOverride;
    public Switch aSwitch;
    public int group;
}

public class Door : MonoBehaviour
{
    [SerializeField]
    // private List<Switch> switches = new List<Switch>();
    private List<DoorSwitch> switches = new List<DoorSwitch>();

    [Header("Debug")]
    [SerializeField]
    private bool debug = false;

    private Animator anim = null;
    private List<bool> firstTruths = new List<bool>();
    private List<bool> secondTruths = new List<bool>();
    private bool overridden = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        firstTruths.Clear();
        secondTruths.Clear();
        overridden = false;

        foreach (DoorSwitch dS in switches) {

            if (!dS.doorOverride) {
                firstTruths.Add(dS.aSwitch.IsActive);
            }
            else if (dS.aSwitch.IsActive) {
                overridden = true;
            }

            if (debug) {
                Debug.DrawLine(transform.position, dS.aSwitch.gameObject.transform.position, Color.green);
            }
        }

        if (!firstTruths.Contains(false) || overridden) {
            anim.SetBool("Open", true);
        }
        else {
            anim.SetBool("Open", false);
        }
    }
}
