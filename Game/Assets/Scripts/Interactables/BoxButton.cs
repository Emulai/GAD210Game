using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxButton : Switch
{
    [SerializeField]
    private GameObject indicator = null;
    [SerializeField]
    private Color doorOpen = Color.green;
    [SerializeField]
    private Color doorClosed = Color.red;

    private TMP_Text indicatorText = null;
    private Image indicatorPanel = null;

    void Start() {
        // Get all parts of the indicator panel
        Canvas c = indicator.GetComponentInChildren<Canvas>();
        indicatorText = c.GetComponentInChildren<TMP_Text>();
        indicatorPanel = c.GetComponentInChildren<Image>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "TriggerBox") {
            // Turn on indicator panel
            isActive = true;
            indicatorText.text = "O";
            indicatorPanel.color = doorOpen;

        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "TriggerBox") {
            // Turn off indicator panel
            isActive = false;
            indicatorText.text = "X";
            indicatorPanel.color = doorClosed;

        }
    }

    public GameObject Indicator {
        set { indicator = value; }
    }
}
