using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct used to handle switch information
[System.Serializable]
public struct DoorSwitch {
    public bool doorOverride;
    public Switch theSwitch;
    public int group;
}

public class Door : MonoBehaviour
{
    [SerializeField]
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
        // Clear last loop's truths
        firstTruths.Clear();
        secondTruths.Clear();
        overridden = false;

        // Loop through each switch attached to this door
        foreach (DoorSwitch dS in switches) {

            // If it isn't an override switch
            if (!dS.doorOverride) {
                // Check its group then add to appropriate truth list
                if (dS.group == 1) {
                    firstTruths.Add(dS.theSwitch.IsActive);
                }
                else {
                    secondTruths.Add(dS.theSwitch.IsActive);
                }
            }
            // Else mark as overridden
            else if (dS.theSwitch.IsActive) {
                overridden = true;
            }

            // Show attached switches 
            if (debug) {
                Debug.DrawLine(transform.position, dS.theSwitch.gameObject.transform.position, Color.green);
            }
        }

        // If first or second truths aren't empty, and don't contain false, or door is overridden, open door. Otherwise keep it closed!
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
