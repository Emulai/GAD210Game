using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DoorSwitch {
    public bool doorOverride;
    public Switch theSwitch;
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
    public List<bool> firstTruths = new List<bool>();
    public List<bool> secondTruths = new List<bool>();
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
                if (dS.group == 1) {
                    firstTruths.Add(dS.theSwitch.IsActive);
                }
                else {
                    secondTruths.Add(dS.theSwitch.IsActive);
                }
            }
            else if (dS.theSwitch.IsActive) {
                overridden = true;
            }

            if (debug) {
                Debug.DrawLine(transform.position, dS.theSwitch.gameObject.transform.position, Color.green);
            }
        }

        if ((firstTruths.Count > 0 && !firstTruths.Contains(false)) || 
            (secondTruths.Count > 0 && !secondTruths.Contains(false)) || 
            overridden) 
        {
            anim.SetBool("Open", true);
        }
        else {
            anim.SetBool("Open", false);
        }
    }

    public List<DoorSwitch> Switches {
        get { return switches; }
        set { switches = value; }
    }
}
